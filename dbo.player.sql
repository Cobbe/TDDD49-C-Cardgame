CREATE TABLE [dbo].[Player] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [name]     NVARCHAR (50) NOT NULL,
    [ai]       BIT           NOT NULL,
    [strength] INT           NOT NULL,
    [pass]     BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

