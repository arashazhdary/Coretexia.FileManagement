using Coretexia.FileManagement.FileSystem.Extensions.DependencyInjections;
using Cortexia.FileManagement.SqlServer.FileStream.Extensions.DependencyInjections;

namespace Cortexia.FileManagement.Endpoint.WebAPI.Extensions.DependencyInjections;

public static class WebAPIFileManagementDependencyInjections
{
    public static IServiceCollection AddWebAPIFileManagement(this IServiceCollection services,
                                                             IConfiguration configuration)
    {
        var fileSystemConfiguration = configuration.GetSection("FileSystem");
        if (fileSystemConfiguration.Exists())
        {
            return services.AddFileSystemFileManagement(fileSystemConfiguration);
        }

        var sqlServerFileStreamConfiguration = configuration.GetSection("SqlServerFileStream");
        if (sqlServerFileStreamConfiguration.Exists())
        {
            return services.AddSqlServerFileStreamFileManagement(sqlServerFileStreamConfiguration);
        }

        throw new InvalidOperationException("Please Config FileManagement First...");
    }
}
