using Coretexia.FileManagement.Abstraction.Models.Common;

namespace Coretexia.FileManagement.Abstraction.Models.DownloadServices;

public class FileManagementDownloadServiceResult : FileManagementResult
{
    public FileManagementDownloadServiceResult()
    {
    }

    public FileManagementDownloadServiceResult(Guid key,
                                               string serviceName,
                                               string name,
                                               string content,
                                               string extension,
                                               DateTime createDate,
                                               byte[] file,
                                               DateTime? modifyDate = null)
        : base(key, serviceName, name, content, extension, createDate, modifyDate)
    {
        File = file;
    }

    public byte[] File { get; set; }
}