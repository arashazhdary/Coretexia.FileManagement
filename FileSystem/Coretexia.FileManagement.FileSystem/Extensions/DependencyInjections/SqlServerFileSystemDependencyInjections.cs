using Coretexia.FileManagement.Abstraction.Services;
using Coretexia.FileManagement.FileSystem.Models.Options;
using Coretexia.FileManagement.FileSystem.Models.SqlServerQueries;
using Coretexia.FileManagement.FileSystem.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Coretexia.FileManagement.FileSystem.Extensions.DependencyInjections;

public static class FileSystemDependencyInjections
{
    public static IServiceCollection AddFileSystemFileManagement(this IServiceCollection services,
                                                                         Action<FileSystemOption> settings)
    {
        FileSystemOption options = new();
        settings(options);
        if (options is null)
            throw new ArgumentNullException("FileSystemOption is not valid");

        addFileSystemFileManagementDependency(services, options);
        return services;
    }

    public static IServiceCollection AddFileSystemFileManagement(this IServiceCollection services,
                                                                          IConfigurationSection configuration)
    {
        FileSystemOption? options = configuration.Get<FileSystemOption>();
        if (options is null)
            throw new ArgumentNullException("Configuration section is not valid");

        addFileSystemFileManagementDependency(services, options);
        return services;
    }

    private static void addFileSystemFileManagementDependency(IServiceCollection services, FileSystemOption options)
    {
        checkOptionsAndCreateInfraIfNeed(options);
        services.AddSingleton(provider => options);
        services.AddScoped<IFileManagement, FileSystemFileManagement>();
    }

    private static void checkOptionsAndCreateInfraIfNeed(FileSystemOption options)
    {
        SqlQuery.SetSchemaAndTableNames(schemaName: options.SchemaName, tableName: options.TableName);
        try
        {
            if (string.IsNullOrWhiteSpace(options.RootPath))
                throw new ArgumentNullException("RootPath is empty");

            if (string.IsNullOrWhiteSpace(options.ConnectionString))
                throw new ArgumentNullException("ConnectionString path is empty");

            if (string.IsNullOrWhiteSpace(options.SchemaName))
                throw new ArgumentNullException("SchemaName path is empty");

            if (string.IsNullOrWhiteSpace(options.TableName))
                throw new ArgumentNullException("TableName path is empty");

            if (!Directory.Exists(options.RootPath))
                Directory.CreateDirectory(options.RootPath);

            using SqlConnection connection = new(options.ConnectionString);

            bool schemaExists = connection.ExecuteScalar<bool>(SqlQuery.CheckSchemaExist, new { options.SchemaName });
            if (!schemaExists && options.AutoCreate)
                connection.Execute(SqlQuery.CreateSchema);
            else if (!schemaExists)
                throw new InvalidOperationException("Schema is not exist and AutoCreate = false");

            bool tableExists = connection.ExecuteScalar<bool>(SqlQuery.CheckTableExist,
                                                              new
                                                              {
                                                                  options.SchemaName,
                                                                  options.TableName
                                                              });
            if (!tableExists && options.AutoCreate)
                connection.Execute(SqlQuery.CreateTable);
            else if (!tableExists)
                throw new InvalidOperationException("Table is not exist and AutoCreate = false");
        }
        catch
        {
            throw;
        }
    }
}
