using Coretexia.FileManagement.Abstraction.Models.Common;

namespace Coretexia.FileManagement.FileSystem.Models.SqlServerModels;

public class FileSystemSqlServerQueryResult : FileManagementResult
{
    public FileSystemSqlServerQueryResult()
    {
    }

    public FileSystemSqlServerQueryResult(Guid key,
                                          string serviceName,
                                          string name,
                                          string content,
                                          string extension,
                                          string path,
                                          bool isDeleted,
                                          DateTime createDate,
                                          DateTime? modifyDate = null)
        : base(key, serviceName, name, content, extension, createDate, modifyDate)
    {
        Path = path;
        IsDeleted = isDeleted;
    }

    public string Path { get; set; }
    public bool IsDeleted { get; set; }

    public void CopyToTarget(FileManagementResult target)
    {
        target.Key = Key;
        target.ServiceName = ServiceName;
        target.Name = Name;
        target.Content = Content;
        target.Extension = Extension;
        target.CreateDate = CreateDate;
        target.ModifyDate = ModifyDate;
        target.Errors = Errors;
    }

    public string GetFileName => $"{Path}\\{Name}_{Key}.{Extension}";
}