namespace Coretexia.FileManagement.Abstraction.Models.UploadServices;

public class FileManagementUploadServiceInput
{

    public FileManagementUploadServiceInput(byte[] file,
                                            string serviceName,
                                            string name,
                                            string content,
                                            string extension)
    {
        if (file is null || file.Length == 0)
            throw new ArgumentException("File is empty");

        if (string.IsNullOrWhiteSpace(serviceName))
            throw new ArgumentException("Service name is empty");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("File name is empty");

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("File content is empty");

        if (string.IsNullOrWhiteSpace(extension))
            throw new ArgumentException("File extension is empty");

        File = file;
        ServiceName = serviceName;
        Name = name;
        Content = content;
        Extension = extension;
    }

    public byte[] File { get; set; }
    public string ServiceName { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Extension { get; set; }
}