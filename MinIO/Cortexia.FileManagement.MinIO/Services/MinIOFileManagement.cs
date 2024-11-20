using Coretexia.FileManagement.Abstraction.Models.DeleteServices;
using Coretexia.FileManagement.Abstraction.Models.DownloadServices;
using Coretexia.FileManagement.Abstraction.Models.UploadServices;
using Coretexia.FileManagement.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cortexia.FileManagement.MinIO.Services;
public class MinIOFileManagement : IFileManagement
{
    public Task<FileManagementDeleteServiceResult?> Delete(FileManagementDeleteServiceInput input)
    {
        throw new NotImplementedException();
    }

    public Task<FileManagementDownloadServiceResult?> Download(FileManagementDownloadServiceInput input)
    {
        throw new NotImplementedException();
    }

    public Task<FileManagementUploadServiceResult?> Upload(FileManagementUploadServiceInput input)
    {
        throw new NotImplementedException();
    }
}
