USE [{{YourDatabase}}]
GO

CREATE TABLE [{{SchemaName}}].[{{TableName}}](
	[Key] [UNIQUEIDENTIFIER] NOT NULL DEFAULT NEWSEQUENTIALID(),
	[ServiceName] [varchar](100) NULL,
	[Name] [NVARCHAR](200) NULL,
	[Content] [VARCHAR](50) NULL,
	[Extension] [VARCHAR](50) NULL,
	[Path] [NVARCHAR](2000) FILESTREAM NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT(0),
	[CreateDate] [DATETIME2] NOT NULL DEFAULT GETDATE(),
	[ModifiedDate] [DATETIME2] NULL,
CONSTRAINT [PK_{{TableName}}] PRIMARY Key
(
	[Key] ASC
) WITH (OPTIMIZE_FOR_SEQUENTIAL_KEY = ON)
)
GO

CREATE OR ALTER  PROCEDURE [{{SchemaName}}].[DownloadFile](@Id UNIQUEIDENTIFIER, @ServiceName VARCHAR(100))
AS
BEGIN

SELECT T.[Key], T.[ServiceName], T.[Name], T.[Content], T.[Extension], T.[Path], T.[IsDeleted],
T.[CreateDate], T.[ModifiedDate]
FROM [{{SchemaName}}].[{{TableName}}] AS T
WHERE T.[Key] = @Key AND T.[ServiceName] = @ServiceName

END
GO

CREATE OR ALTER PROCEDURE [{{SchemaName}}].[UploadFile](@ServiceName VARCHAR(100), @Name NVARCHAR(200), 
 @Content VARCHAR(50), @Extension VARCHAR(50), @Path NVARCHAR(2000))
AS
BEGIN

INSERT INTO [{{SchemaName}}].[{{TableName}}]
([ServiceName], [Name], [Content], [Extension], [Path])
OUTPUT INSERTED.[Key], INSERTED.[ServiceName], INSERTED.[Name], INSERTED.[Content], INSERTED.[Extension], INSERTED.[Path], 
INSERTED.[CreateDate], INSERTED.[ModifiedDate]
VALUES (@ServiceName, @Name, @Content, @Extension, @Path)

END
GO

CREATE OR ALTER PROCEDURE [{{SchemaName}}].[DeleteFile](@Key UNIQUEIDENTIFIER, @ServiceName VARCHAR(100), @IsDeleted BIT)
AS
BEGIN

UPDATE [{{SchemaName}}].[{{TableName}}]
SET [IsDeleted] = @IsDeleted
OUTPUT DELETED.[Key], DELETED.[ServiceName], DELETED.[Name], DELETED.[Content], DELETED.[Extension], DELETED.[Path], 
DELETED.[CreateDate], DELETED.[ModifiedDate]
WHERE [Key] = @Key AND [ServiceName] = @ServiceName

END
GO