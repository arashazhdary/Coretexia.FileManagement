namespace Coretexia.FileManagement.Abstraction.Models.Common;

public abstract class FileManagementResult
{

    protected FileManagementResult()
    {
    }

    protected FileManagementResult(Guid key,
                                   string serviceName,
                                   string name,
                                   string content,
                                   string extension,
                                   DateTime createDate,
                                   DateTime? modifyDate = null)
    {
        Key = key;
        Name = name;
        Content = content;
        ServiceName = serviceName;
        Extension = extension;
        CreateDate = createDate;
        ModifyDate = modifyDate;
    }

    public Guid Key { get; set; }
    public string ServiceName { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public string Extension { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? ModifyDate { get; set; }
    public bool IsSuccess => !Errors.Any();
    public List<FileManagementError> Errors { get; set; } = new();


}