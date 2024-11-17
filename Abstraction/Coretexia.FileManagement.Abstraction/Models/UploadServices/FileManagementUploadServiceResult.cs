using Coretexia.FileManagement.Abstraction.Models.Common;

namespace Coretexia.FileManagement.Abstraction.Models.UploadServices;

public class FileManagementUploadServiceResult : FileManagementResult
{
    public FileManagementUploadServiceResult()
    {
    }

    public FileManagementUploadServiceResult(Guid key,
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