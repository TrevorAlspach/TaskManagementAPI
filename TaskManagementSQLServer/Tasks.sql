CREATE TABLE [dbo].[Tasks]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[ListId] UNIQUEIDENTIFIER NOT NULL,
	[IsCompleted] BIT NOT NULL,
	[Priority] BIT NOT NULL,
	[Name] VARCHAR(30),
	[Description] VARCHAR(200),
	[Deadline] DATETIMEOFFSET NOT NULL,
)
