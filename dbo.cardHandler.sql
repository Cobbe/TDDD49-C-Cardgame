CREATE TABLE [dbo].[CardHandler] (
    [id]       INT  IDENTITY (1, 1) NOT NULL,
    [type]     NVARCHAR(50) NOT NULL,
    [playerId] INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

