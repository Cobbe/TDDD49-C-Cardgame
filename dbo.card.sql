CREATE TABLE [dbo].[Card] (
    [id]          INT IDENTITY NOT NULL,
    [description] NVARCHAR(50) NULL,
    [name]        NVARCHAR(50) NOT NULL,
    [strength]    INT  NOT NULL,
    [image]       NVARCHAR(50) NOT NULL,
    [cardHandlerId] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([id] ASC)
);

