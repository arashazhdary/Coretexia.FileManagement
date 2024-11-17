namespace Cortexia.FileManagement.SqlServer.FileStream.Models.SqlServerQueries;

public static class SqlQuery
{
    public static void SetSchemaAndTableNames(string schemaName, string tableName)
    {
        _uploadFile = replaceMarkerWithRealName(_uploadFile, schemaName, tableName);
        _deleteFile = replaceMarkerWithRealName(_deleteFile, schemaName, tableName);
        _downloadFile = replaceMarkerWithRealName(_downloadFile, schemaName, tableName);
    }

    private static string replaceMarkerWithRealName(string template, string schemaName, string tableName)
        => template.Replace(schemaNameMarker, schemaName).Replace(tableNameMarker, tableName);

    private static string schemaNameMarker = "{{SchemaName}}";
    private static string tableNameMarker = "{{TableName}}";

    public static string CheckFileStreamEnable { get; } = @"SELECT CAST(1 AS BIT) FROM sys.configurations 
WHERE [name] = 'filestream access level' AND [value_in_use] > 0";

    public static string CheckSchemaExist { get; } = @"SELECT CAST(1 AS BIT) FROM sys.schemas AS S 
WHERE S.[name] = @SchemaName";

    public static string CheckTableExist { get; } = @"SELECT CAST(1 AS BIT) FROM sys.tables AS T 
WHERE T.[name] = @TableName AND T.schema_id = SCHEMA_ID(@SchemaName)";

    public static string _uploadFile = @"INSERT INTO [{{SchemaName}}].[{{TableName}}]
([ServiceName], [Name], [Content], [Extension], [File])
OUTPUT INSERTED.[Key], INSERTED.[ServiceName], INSERTED.[Name], INSERTED.[Content], INSERTED.[Extension], INSERTED.[File], 
INSERTED.[CreateDate], INSERTED.[ModifiedDate]
VALUES (@ServiceName, @Name, @Content, @Extension, @File)";
    public static string UploadFile => _uploadFile;

    public static string _deleteFile = @"DELETE FROM [{{SchemaName}}].[{{TableName}}]
OUTPUT DELETED.[Key], DELETED.[ServiceName], DELETED.[Name], DELETED.[Content], DELETED.[Extension], 
DELETED.[CreateDate], DELETED.[ModifiedDate]
WHERE [Key] = @Key AND [ServiceName] = @ServiceName";
    public static string DeleteFile => _deleteFile;

    public static string _downloadFile = @"SELECT T.[Key], T.[ServiceName], T.[Name], T.[Content], T.[Extension], T.[File], 
T.[CreateDate], T.[ModifiedDate]
FROM [{{SchemaName}}].[{{TableName}}] AS T
WHERE T.[Key] = @Key AND T.[ServiceName] = @ServiceName";
    public static string DownloadFile => _downloadFile;
}
