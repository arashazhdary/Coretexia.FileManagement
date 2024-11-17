USE [master]
GO

CREATE DATABASE [{{YourDatabaseName}}FileManagement]
 ON  PRIMARY 
( NAME = N'{{YourDatabaseName}}FileManagement', FILENAME = N'D:\DataFile\{{YourDatabaseName}}FileManagement\{{YourDatabaseName}}FileManagement.mdf' 
, SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB ), 
 FILEGROUP [{{YourDatabaseName}}FileManagementDataFG]  DEFAULT
( NAME = N'{{YourDatabaseName}}FileManagementDataFile01', FILENAME = N'D:\DataFile\{{YourDatabaseName}}FileManagement\{{YourDatabaseName}}FileManagement_DataFile01.ndf' 
, SIZE = 1048576KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1048576KB ), 
 FILEGROUP [{{YourDatabaseName}}FileManagementFilestreamFG] CONTAINS FILESTREAM  DEFAULT
( NAME = N'{{YourDatabaseName}}FileManagementContainer', FILENAME = N'D:\DataFile\{{YourDatabaseName}}FileManagement\{{YourDatabaseName}}FileManagementContainer')
 LOG ON 
( NAME = N'{{YourDatabaseName}}FileManagement_log', FILENAME = N'D:\DataFile\{{YourDatabaseName}}FileManagement\{{YourDatabaseName}}FileManagement_log.ldf' 
, SIZE = 1048576KB , MAXSIZE = 2048GB , FILEGROWTH = 1048576KB )
GO

ALTER DATABASE [{{YourDatabaseName}}FileManagement] MODIFY FILEGROUP [{{YourDatabaseName}}FileManagementDataFG] AUTOGROW_ALL_FILES
GO

USE [{{YourDatabaseName}}FileManagement]
GO

CREATE TABLE [{{SchemaName}}].[{{TableName}}](
	[Key] [UNIQUEIDENTIFIER] ROWGUIDCOL NOT NULL DEFAULT NEWSEQUENTIALID(),
	[ServiceName] [varchar](100) NULL,
	[Name] [NVARCHAR](200) NULL,
	[Content] [VARCHAR](50) NULL,
	[Extension] [VARCHAR](50) NULL,
	[File] [VARBINARY](max) FILESTREAM NOT NULL,
	[CreateDate] [DATETIME2] NOT NULL DEFAULT GETDATE(),
	[ModifiedDate] [DATETIME2] NULL,
CONSTRAINT [PK_{{TableName}}] PRIMARY Key
(
	[Key] ASC
) WITH (OPTIMIZE_FOR_SEQUENTIAL_KEY = ON) ON [{{YourDatabaseName}}FileManagementDataFG]
) ON [{{YourDatabaseName}}FileManagementDataFG] FILESTREAM_ON [{{YourDatabaseName}}FileManagementFilestreamFG]
GO

CREATE OR ALTER  PROCEDURE [{{SchemaName}}].[DownloadFile](@Id UNIQUEIDENTIFIER, @ServiceName VARCHAR(100))
AS
BEGIN

SELECT T.[Key], T.[ServiceName], T.[Name], T.[Content], T.[Extension], T.[File], 
T.[CreateDate], T.[ModifiedDate]
FROM [{{SchemaName}}].[{{TableName}}] AS T
WHERE T.[Key] = @Key AND T.[ServiceName] = @ServiceName

END
GO

CREATE OR ALTER   PROCEDURE [{{SchemaName}}].[UploadFile](@ServiceName VARCHAR(100), @Name NVARCHAR(200), 
 @Content VARCHAR(50), @Extension VARCHAR(50), @File VARBINARY(MAX))
AS
BEGIN

INSERT INTO [{{SchemaName}}].[{{TableName}}]
([ServiceName], [Name], [Content], [Extension], [File])
OUTPUT INSERTED.[Key], INSERTED.[ServiceName], INSERTED.[Name], INSERTED.[Content], INSERTED.[Extension], INSERTED.[File], 
INSERTED.[CreateDate], INSERTED.[ModifiedDate]
VALUES (@ServiceName, @Name, @Content, @Extension, @File)

END
GO

CREATE OR ALTER PROCEDURE [{{SchemaName}}].[DeleteFile](@Key UNIQUEIDENTIFIER, @ServiceName VARCHAR(100))
AS
BEGIN

DELETE FROM [{{SchemaName}}].[{{TableName}}]
OUTPUT DELETED.[Key], DELETED.[ServiceName], DELETED.[Name], DELETED.[Content], DELETED.[Extension],
DELETED.[CreateDate], DELETED.[ModifiedDate]
WHERE [Key] = @Key AND [ServiceName] = @ServiceName

END
GO