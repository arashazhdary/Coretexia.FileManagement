namespace Coretexia.FileManagement.FileSystem.Models.SqlServerQueries;

public static class SqlQuery
{
    public static void SetSchemaAndTableNames(string schemaName, string tableName)
    {
        _createSchema = replaceMarkerWithRealName(_createSchema, schemaName, tableName);
        _createTable = replaceMarkerWithRealName(_createTable, schemaName, tableName);
        _uploadFile = replaceMarkerWithRealName(_uploadFile, schemaName, tableName);
        _deleteFile = replaceMarkerWithRealName(_deleteFile, schemaName, tableName);
        _downloadFile = replaceMarkerWithRealName(_downloadFile, schemaName, tableName);
        _deleteFileWhenUploadFail = replaceMarkerWithRealName(_deleteFileWhenUploadFail, schemaName, tableName);
    }

    private static string replaceMarkerWithRealName(string template, string schemaName, string tableName)
        => template.Replace(schemaNameMarker, schemaName).Replace(tableNameMarker, tableName);

    private static string schemaNameMarker = "{{SchemaName}}";
    private static string tableNameMarker = "{{TableName}}";

    public static string CheckSchemaExist { get; } = @"SELECT CAST(1 AS BIT) FROM sys.schemas AS S 
WHERE S.[name] = @SchemaName";

    public static string _createSchema = @"CREATE SCHEMA [{{SchemaName}}]";
    public static string CreateSchema => _createSchema;

    public static string CheckTableExist { get; } = @"SELECT CAST(1 AS BIT) FROM sys.tables AS T 
WHERE T.[name] = @TableName AND T.schema_id = SCHEMA_ID(@SchemaName)";

    public static string _createTable = @"CREATE TABLE [{{SchemaName}}].[{{TableName}}](
[Key] [UNIQUEIDENTIFIER] ROWGUIDCOL NOT NULL DEFAULT NEWSEQUENTIALID(),
[ServiceName] [varchar](100) NULL,
[Name] [NVARCHAR](200) NULL,
[Content] [VARCHAR](50) NULL,
[Extension] [VARCHAR](50) NULL,
[Path] [NVARCHAR](2000) NOT NULL,
[IsDeleted] BIT NOT NULL DEFAULT(0),
[CreateDate] [DATETIME2] NOT NULL DEFAULT GETDATE(),
[ModifiedDate] [DATETIME2] NULL,
CONSTRAINT [PK_{{TableName}}] PRIMARY Key
(
	[Key] ASC
) WITH (OPTIMIZE_FOR_SEQUENTIAL_KEY = ON)
)";
    public static string CreateTable => _createTable;

    public static string _uploadFile = @"INSERT INTO [{{SchemaName}}].[{{TableName}}]
([ServiceName], [Name], [Content], [Extension], [Path])
OUTPUT INSERTED.[Key], INSERTED.[ServiceName], INSERTED.[Name], INSERTED.[Content], INSERTED.[Extension], INSERTED.[Path], 
INSERTED.[CreateDate], INSERTED.[ModifiedDate]
VALUES (@ServiceName, @Name, @Content, @Extension, @Path)";
    public static string UploadFile => _uploadFile;

    public static string _deleteFileWhenUploadFail = @"DELETE FROM [{{SchemaName}}].[{{TableName}}]
WHERE [Key] = @Key AND [ServiceName] = @ServiceName";
    public static string DeleteFileWhenUploadFail => _deleteFileWhenUploadFail;

    public static string _deleteFile = @"UPDATE [{{SchemaName}}].[{{TableName}}]
SET [IsDeleted] = @IsDeleted
OUTPUT DELETED.[Key], DELETED.[ServiceName], DELETED.[Name], DELETED.[Content], DELETED.[Extension], DELETED.[Path], 
DELETED.[CreateDate], DELETED.[ModifiedDate]
WHERE [Key] = @Key AND [ServiceName] = @ServiceName";
    public static string DeleteFile => _deleteFile;

    public static string _downloadFile = @"SELECT T.[Key], T.[ServiceName], T.[Name], T.[Content], T.[Extension], T.[Path], T.[IsDeleted],
T.[CreateDate], T.[ModifiedDate]
FROM [{{SchemaName}}].[{{TableName}}] AS T
WHERE T.[Key] = @Key AND T.[ServiceName] = @ServiceName";
    public static string DownloadFile => _downloadFile;
}