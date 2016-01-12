CREATE TABLE [dbo].[LogicEngine] (
    [Id]                INT IDENTITY (1, 1) NOT NULL,
    [round]             INT DEFAULT ((0)) NOT NULL,
    [wonBattlesPlayer1] INT DEFAULT ((0)) NOT NULL,
    [wonBattlesPlayer2] INT DEFAULT ((0)) NOT NULL,
    [state]             INT DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

