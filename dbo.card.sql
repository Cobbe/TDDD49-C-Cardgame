CREATE TABLE [dbo].[Card] (
    [id]          INT  IDENTITY (1, 1) NOT NULL,
    [description] TEXT NULL,
    [name]        TEXT NOT NULL,
    [strength]    INT  NOT NULL,
    [image]       TEXT NOT NULL,
    [cardHandlerId] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([id] ASC)
);

