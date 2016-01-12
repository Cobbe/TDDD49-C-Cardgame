CREATE TABLE [dbo].[LogicEngine]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [round] INT NOT NULL DEFAULT 0, 
    [wonBattlesPlayer1] INT NOT NULL DEFAULT 0, 
    [wonBattlesPlayer2] INT NOT NULL DEFAULT 0
	
)
