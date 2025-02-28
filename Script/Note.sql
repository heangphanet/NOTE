CREATE TABLE [dbo].[Notes](
	[Id] [int] IDENTITY(1,1) NOT NULL Primary Key,
	[UserID] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](100) NULL,
	[CreatedAt] [nvarchar](20) NOT NULL,
	[UpdatedAt] [nvarchar](20) NULL,
)


