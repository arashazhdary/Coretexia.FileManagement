using Coretexia.FileManagement.Abstraction.Models.DeleteServices;
using Coretexia.FileManagement.Abstraction.Models.DownloadServices;
using Coretexia.FileManagement.Abstraction.Models.UploadServices;

namespace Coretexia.FileManagement.Abstraction.Services;

public interface IFileManagement
{
    Task<FileManagementUploadServiceResult?> Upload(FileManagementUploadServiceInput input);
    Task<FileManagementDeleteServiceResult?> Delete(FileManagementDeleteServiceInput input);
    Task<FileManagementDownloadServiceResult?> Download(FileManagementDownloadServiceInput input);
}