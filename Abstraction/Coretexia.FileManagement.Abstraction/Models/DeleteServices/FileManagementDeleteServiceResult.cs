using Coretexia.FileManagement.Abstraction.Models.Common;

namespace Coretexia.FileManagement.Abstraction.Models.DeleteServices;

public class FileManagementDeleteServiceResult : FileManagementResult
{
    public FileManagementDeleteServiceResult()
    {
    }

    public FileManagementDeleteServiceResult(Guid key,
                                             string serviceName,
                                             string name,
                                             string content,
                                             string extension,
                                             DateTime createDate,
                                             DateTime? modifyDate = null)
        : base(key, serviceName, name, content, extension, createDate, modifyDate)
    {
    }
}