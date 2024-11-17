namespace Cortexia.FileManagement.SqlServer.FileStream.Models.Options;

public class SqlServerFileStreamOption
{
    public string ConnectionString { get; set; } = null!;
    public string SchemaName { get; set; } = null!;
    public string TableName { get; set; } = null!;
}
