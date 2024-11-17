namespace Coretexia.FileManagement.FileSystem.Models.Options;

public class FileSystemOption
{
    public string RootPath { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
    public string SchemaName { get; set; } = null!;
    public string TableName { get; set; } = null!;
    public bool AutoCreate { get; set; }
}
