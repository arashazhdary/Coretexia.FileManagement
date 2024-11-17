using System.Data;
using System.Runtime.CompilerServices;
using Coretexia.FileManagement.Abstraction.Models.Common;
using Coretexia.FileManagement.Abstraction.Models.DeleteServices;
using Coretexia.FileManagement.Abstraction.Models.DownloadServices;
using Coretexia.FileManagement.Abstraction.Models.UploadServices;
using Coretexia.FileManagement.Abstraction.Services;
using Cortexia.FileManagement.SqlServer.FileStream.Extensions.Exceptions;
using Cortexia.FileManagement.SqlServer.FileStream.Models.Options;
using Cortexia.FileManagement.SqlServer.FileStream.Models.SqlServerQueries;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Cortexia.FileManagement.SqlServer.FileStream.Services;

public class SqlServerFileStreamFileManagement : IFileManagement
{
    private readonly SqlServerFileStreamOption _options;
    private readonly ILogger<SqlServerFileStreamFileManagement> _logger;

    public SqlServerFileStreamFileManagement(SqlServerFileStreamOption options,
                                             ILogger<SqlServerFileStreamFileManagement> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<FileManagementUploadServiceResult?> Upload(FileManagementUploadServiceInput input)
    {
        using var connection = getConnection();
        return await executeQuery(
           func: async () => await connection.QueryFirstOrDefaultAsync<FileManagementUploadServiceResult>(SqlQuery.UploadFile,
                input, commandType: CommandType.Text),
           serviceName: input.ServiceName
        );
    }

    public async Task<FileManagementDeleteServiceResult?> Delete(FileManagementDeleteServiceInput input)
    {
        using var connection = getConnection();
        return await executeQuery(
           func: async () => await connection.QueryFirstOrDefaultAsync<FileManagementDeleteServiceResult>(SqlQuery.DeleteFile,
                input, commandType: CommandType.Text),
           serviceName: input.ServiceName,
           key: input.Key
        );
    }

    public async Task<FileManagementDownloadServiceResult?> Download(FileManagementDownloadServiceInput input)
    {
        using var connection = getConnection();
        return await executeQuery(
           func: async () => await connection.QueryFirstOrDefaultAsync<FileManagementDownloadServiceResult>(SqlQuery.DownloadFile,
                input, commandType: CommandType.Text),
           serviceName: input.ServiceName,
           key: input.Key);
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
            using var connection = getConnection();
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

    private SqlConnection getConnection() => new SqlConnection(_options.ConnectionString);
}
