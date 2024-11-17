namespace Coretexia.FileManagement.Abstraction.Models.DeleteServices;

public class FileManagementDeleteServiceInput
{

    public FileManagementDeleteServiceInput(Guid key, string serviceName)
    {
        if (key == Guid.Empty)
            throw new ArgumentException("File key is empty");

        if (string.IsNullOrWhiteSpace(serviceName))
            throw new ArgumentException("Service name is empty");

        Key = key;
        ServiceName = serviceName;
    }

    public Guid Key { get; private set; }
    public string ServiceName { get; private set; }
}