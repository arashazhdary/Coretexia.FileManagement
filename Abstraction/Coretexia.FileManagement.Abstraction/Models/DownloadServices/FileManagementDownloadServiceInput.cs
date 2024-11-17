namespace Coretexia.FileManagement.Abstraction.Models.DownloadServices;

public class FileManagementDownloadServiceInput
{
    public FileManagementDownloadServiceInput(Guid key, string serviceName)
    {
        if (key == Guid.Empty)
            throw new ArgumentException("File key is empty");

        if (string.IsNullOrWhiteSpace(serviceName))
            throw new ArgumentException("Service name is empty");

        Key = key;
        ServiceName = serviceName;
    }

    public Guid Key { get; set; }
    public string ServiceName { get; private set; }
}