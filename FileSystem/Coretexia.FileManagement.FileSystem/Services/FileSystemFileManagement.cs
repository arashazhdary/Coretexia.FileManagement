using Coretexia.FileManagement.Abstraction.Models.Common;
using Coretexia.FileManagement.Abstraction.Models.DeleteServices;
using Coretexia.FileManagement.Abstraction.Models.DownloadServices;
using Coretexia.FileManagement.Abstraction.Models.UploadServices;
using Coretexia.FileManagement.Abstraction.Services;
using Coretexia.FileManagement.FileSystem.Extensions.Exceptions;
using Coretexia.FileManagement.FileSystem.Models.Options;
using Coretexia.FileManagement.FileSystem.Models.SqlServerModels;
using Coretexia.FileManagement.FileSystem.Models.SqlServerQueries;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Runtime.CompilerServices;

namespace Coretexia.FileManagement.FileSystem.Services;

public class FileSystemFileManagement : IFileManagement
{
    private readonly FileSystemOption _options;
    private readonly ILogger<FileSystemFileManagement> _logger;

    public FileSystemFileManagement(FileSystemOption options,
                                    ILogger<FileSystemFileManagement> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<FileManagementUploadServiceResult?> Upload(FileManagementUploadServiceInput input)
    {
        FileManagementUploadServiceResult? result = new();

        DateTime now = DateTime.Now;
        string path = $"{_options.RootPath}\\{input.ServiceName}\\{now.Year}\\{now.Month}\\{now.Day}\\{now.Hour}";

        #region Save file information in SqlServer

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        FileSystemSqlServerCommandInput fileSystemInput = new(serviceName: input.ServiceName,
                                                              name: input.Name,
                                                              content: input.Content,
                                                              extension: input.Extension,
                                                              path: path);

        using var connection = getConnection();
        var sqlResult = await executeQuery(
           func: async () => await connection.QueryFirstOrDefaultAsync<FileSystemSqlServerQueryResult>(SqlQuery.UploadFile,
                fileSystemInput, commandType: CommandType.Text),
           serviceName: input.ServiceName);

        if (sqlResult is null)
            return null;

        if (!sqlResult.IsSuccess)
        {
            sqlResult.CopyToTarget(result);
            return result;
        }

        #endregion

        #region Save file in FileSystem and if fail rollback IsDeleted in SqlServer

        try
        {
            sqlResult.CopyToTarget(result);
            await File.WriteAllBytesAsync(sqlResult.GetFileName, input.File);
        }
        catch (Exception ex)
        {
            result.Errors.Add(new(ex.GetInnerMessage(), ex));
            _logger.LogError(ex, "Error happen In {Method} when save file to disk - ServiceName: {ServiceName} - Key: {Key}", "Upload",
                 input.ServiceName, sqlResult.Key);

            await executeQuery(
                func: async () => await connection.ExecuteAsync(SqlQuery.DeleteFileWhenUploadFail,
                    new
                    {
                        sqlResult.Key,
                        sqlResult.ServiceName
                    }, commandType: CommandType.Text),
                serviceName: input.ServiceName,
                key: sqlResult.Key);

        }

        #endregion

        return result;
    }

    public async Task<FileManagementDeleteServiceResult?> Delete(FileManagementDeleteServiceInput input)
    {
        FileManagementDeleteServiceResult? result = new();

        #region Delete file record and get file information from SqlServer

        using var connection = getConnection();
        var sqlResult = await executeQuery(
           func: async () => await connection.QueryFirstOrDefaultAsync<FileSystemSqlServerQueryResult>(SqlQuery.DeleteFile,
                new
                {
                    input.Key,
                    input.ServiceName,
                    IsDeleted = true,
                }, commandType: CommandType.Text),
           serviceName: input.ServiceName,
           key: input.Key);

        if (sqlResult is null)
            return null;

        if (!sqlResult.IsSuccess)
        {
            sqlResult.CopyToTarget(result);
            return result;
        }

        #endregion

        #region delete file from FileSystem and if fail rollback IsDeleted in SqlServer

        try
        {
            sqlResult.CopyToTarget(result);
            await Task.Run(() => File.Delete(sqlResult.GetFileName));
        }
        catch (Exception ex)
        {
            result.Errors.Add(new(ex.GetInnerMessage(), ex));
            _logger.LogError(ex, "Error happen In {Method} - ServiceName: {ServiceName} - Key: {Key}", "Delete", input.ServiceName, input.Key);

            await executeQuery(
                func: async () => await connection.ExecuteAsync(SqlQuery.DeleteFile,
                    new
                    {
                        input.Key,
                        input.ServiceName,
                        IsDeleted = false,
                    }, commandType: CommandType.Text),
                serviceName: input.ServiceName,
                key: sqlResult.Key);
        }

        #endregion

        return result;
    }

    public async Task<FileManagementDownloadServiceResult?> Download(FileManagementDownloadServiceInput input)
    {
        FileManagementDownloadServiceResult? result = new();

        #region Get file information from SqlServer

        using var connection = getConnection();
        var sqlResult = await executeQuery(
           func: async () => await connection.QueryFirstOrDefaultAsync<FileSystemSqlServerQueryResult>(SqlQuery.DownloadFile,
                input, commandType: CommandType.Text),
           serviceName: input.ServiceName,
           key: input.Key);

        if (sqlResult is null)
            return null;

        if (!sqlResult.IsSuccess)
        {
            sqlResult.CopyToTarget(result);
            return result;
        }

        #endregion

        #region Get file from FileSystem

        try
        {
            sqlResult.CopyToTarget(result);
            result.File = await File.ReadAllBytesAsync(sqlResult.GetFileName);
        }
        catch (Exception ex)
        {
            result.Errors.Add(new(ex.GetInnerMessage(), ex));
            _logger.LogError(ex, "Error happen In {Method} - ServiceName: {ServiceName} - Key: {Key}", "Download", input.ServiceName, input.Key);
        }

        #endregion

        return result;
    }

    private async Task<TResult?> executeQuery<TResult>(Func<Task<TResult?>> func,
                                                       string serviceName,
                                                       Guid? key = null,
                                                       [CallerMemberName] string memeberName = "")
        where TResult : FileManagementResult
    {
        TResult? result = null;
        try
        {
            result = await func();
            return result;
        }
        catch (Exception ex)
        {
            result = Activator.CreateInstance<TResult>();
            result.Errors.Add(new(ex.GetInnerMessage(), ex));
            _logger.LogError(ex, "Error happen In {Method} - ServiceName: {ServiceName} - Key: {Key}", memeberName, serviceName, key);
        }
        return result;
    }

    private async Task executeQuery(Func<Task> func,
                                    string serviceName,
                                    Guid? key = null,
                                    [CallerMemberName] string memeberName = "")
    {
        try
        {
            await func();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error happen In Rollback {Method} - ServiceName: {ServiceName} - Key: {Key}", memeberName, serviceName, key);
        }
    }
    private SqlConnection getConnection() => new SqlConnection(_options.ConnectionString);

}
