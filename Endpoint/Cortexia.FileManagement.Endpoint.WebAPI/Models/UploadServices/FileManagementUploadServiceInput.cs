namespace Cortexia.FileManagement.Endpoint.WebAPI.Models.UploadServices;

public class FileManagementUploadServiceModel
{
    public IFormFile File { get; set; }
    public string ServiceName { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Extension { get; set; }
}