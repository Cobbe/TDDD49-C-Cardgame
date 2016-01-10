CREATE TABLE [dbo].[Player] (
    [id]   INT  IDENTITY (1, 1) NOT NULL,
    [name] NVARCHAR(50) NOT NULL,
    [ai]   BIT  NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

