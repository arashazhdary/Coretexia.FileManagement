namespace Coretexia.FileManagement.FileSystem.Models.SqlServerModels;

public class FileSystemSqlServerCommandInput
{
    public FileSystemSqlServerCommandInput(string path,
                                           string serviceName,
                                           string name,
                                           string content,
                                           string extension)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("File is empty");

        if (string.IsNullOrWhiteSpace(serviceName))
            throw new ArgumentException("Service name is empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("File name is empty");

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("File content is empty");

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("File extension is empty");

        Path = path;
        ServiceName = serviceName;
        Name = name;
        Content = content;
        Extension = extension;
    }

    public string Path { get; set; }
    public string ServiceName { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Extension { get; set; }
}