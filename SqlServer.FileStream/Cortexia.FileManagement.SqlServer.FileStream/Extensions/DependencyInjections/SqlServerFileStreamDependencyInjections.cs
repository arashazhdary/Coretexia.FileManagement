using Coretexia.FileManagement.Abstraction.Services;
using Cortexia.FileManagement.SqlServer.FileStream.Models.Options;
using Cortexia.FileManagement.SqlServer.FileStream.Models.SqlServerQueries;
using Cortexia.FileManagement.SqlServer.FileStream.Services;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cortexia.FileManagement.SqlServer.FileStream.Extensions.DependencyInjections;

public static class SqlServerFileStreamDependencyInjections
{
    public static IServiceCollection AddSqlServerFileStreamFileManagement(this IServiceCollection services,
                                                                          Action<SqlServerFileStreamOption> settings)
    {
        SqlServerFileStreamOption options = new();
        settings(options);
        if (options is null)
            throw new ArgumentNullException("SqlServerFileStreamOption is not valid");

        addSqlServerFileStreamFileManagementDependency(services, options);
        return services;
    }

    public static IServiceCollection AddSqlServerFileStreamFileManagement(this IServiceCollection services,
                                                                          IConfigurationSection configuration)
    {
        SqlServerFileStreamOption? options = configuration.Get<SqlServerFileStreamOption>();
        if (options is null)
            throw new ArgumentNullException("Configuration section is not valid");

        addSqlServerFileStreamFileManagementDependency(services, options);
        return services;
    }

    private static void addSqlServerFileStreamFileManagementDependency(IServiceCollection services, SqlServerFileStreamOption options)
    {
        checkOptionsAndCreateInfraIfNeed(options);
        services.AddSingleton(provider => options);
        services.AddScoped<IFileManagement, SqlServerFileStreamFileManagement>();
    }

    private static void checkOptionsAndCreateInfraIfNeed(SqlServerFileStreamOption options)
    {
        SqlQuery.SetSchemaAndTableNames(schemaName: options.SchemaName, tableName: options.TableName);
        try
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
                throw new ArgumentNullException("ConnectionString path is empty");

            if (string.IsNullOrWhiteSpace(options.SchemaName))
                throw new ArgumentNullException("SchemaName path is empty");

            if (string.IsNullOrWhiteSpace(options.TableName))
                throw new ArgumentNullException("TableName path is empty");

            using SqlConnection connection = new(options.ConnectionString);

            bool fileStreamIsEnable = connection.ExecuteScalar<bool>(SqlQuery.CheckFileStreamEnable);
            if (!fileStreamIsEnable)
                throw new InvalidOperationException("FileStream is not enable in server.");

            bool schemaExists = connection.ExecuteScalar<bool>(SqlQuery.CheckSchemaExist, new { options.SchemaName });
            if (!schemaExists)
                throw new InvalidOperationException("Schema is not exist.");

            bool tableExists = connection.ExecuteScalar<bool>(SqlQuery.CheckTableExist,
                                                              new
                                                              {
                                                                  options.SchemaName,
                                                                  options.TableName
                                                              });
            if (!tableExists)
                throw new InvalidOperationException("Table is not exist.");
        }
        catch
        {
            throw;
        }
    }
}
