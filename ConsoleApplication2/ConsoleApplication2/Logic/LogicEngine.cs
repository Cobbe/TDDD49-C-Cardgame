using GwentStandalone;
using GwentStandAlone;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;


namespace Logic
{
    [Table(Name = "LogicEngine")]
    class LogicEngine
    {
        private static LogicEngine logicEngine;

        [Column(IsPrimaryKey = true)]
        private int id;
        [Column]
        private int round;
        [Column]
        private int wonBattlesPlayer1;
        [Column]
        private int wonBattlesPlayer2;
        [Column]
        public GameState state;

        public LogicEngine() : base()
        {

        }

        public static LogicEngine getInstance()
        {
            if(logicEngine == null)
            {
                return logicEngine = getDBInstance();
            }
            return logicEngine;
        }

        public Table<Player> getPlayers()
        {
            Table<Player> players = Program.db.GetTable<Player>();

            return players;
        }
        
        public static Player getPlayer1()
        {
            Table<Player> players = Program.db.GetTable<Player>();

            return players.First();
        }

        public static Player getPlayer2()
        {
            Table<Player> players = Program.db.GetTable<Player>();
            var query = from player in players select player;
            return query.OrderByDescending(player => player.id).First();
        }

        public static int getRound()
        {
            LogicEngine temp = getDBInstance();
            if(temp == null)
            {
                return -1;
            }
            return getDBInstance().round;
        }

        public static GameState getState()
        {
            LogicEngine temp = getDBInstance();
            if (temp == null)
            {
                return GameState.Problem;
            }
            return getDBInstance().state;
        }

        public static int getWonBattlesPlayer1()
        {
            LogicEngine temp = getDBInstance();
            if (temp == null)
            {
                return -1;
            }
            return getDBInstance().wonBattlesPlayer1;
        }

        public static int getWonBattlesPlayer2()
        {
            LogicEngine temp = getDBInstance();
            if (temp == null)
            {
                return -1;
            }
            return getDBInstance().wonBattlesPlayer2;
        }

        public static void nextRound()
        {
            getPlayer1().setPass(false);
            getPlayer2().setPass(false);
            getPlayer1().setStrength(0);
            getPlayer2().setStrength(0);
            getPlayer1().movePlayedCardsToUsed();
            getPlayer2().movePlayedCardsToUsed();
            Program.db.ExecuteCommand("UPDATE LogicEngine SET round ={0} WHERE id = {1}", getRound() + 1, getInstance().id);
            Program.db.Refresh(RefreshMode.OverwriteCurrentValues, Program.db.GetTable<LogicEngine>());
        }

        public static void player1Won()
        {
            Program.db.ExecuteCommand("UPDATE LogicEngine SET wonBattlesPlayer1 ={0} WHERE id = {1}", getWonBattlesPlayer1() + 1, getInstance().id);
            Program.db.Refresh(RefreshMode.OverwriteCurrentValues, Program.db.GetTable<LogicEngine>());
        }

        public static void player2Won()
        {
            Program.db.ExecuteCommand("UPDATE LogicEngine SET wonBattlesPlayer2 ={0} WHERE id = {1}", getWonBattlesPlayer2() + 1, getInstance().id);
            Program.db.Refresh(RefreshMode.OverwriteCurrentValues, Program.db.GetTable<LogicEngine>());
        }

        public static GameState determineRound()
        {
            if(getRound() > 3)
            {
                Console.WriteLine("It is over");
                nextRound();
                return GameState.EndGame;
            }
            //Console.WriteLine("P1Pass: " + getPlayer1().pass + " P2Pass: " + getPlayer2().pass);
            if (getPlayer1().pass == true && getPlayer2().pass == true)
            {
                if(getPlayer1().strength > getPlayer2().strength)
                {
                    player1Won();
                } else
                {
                    player2Won();
                }
                if(getWonBattlesPlayer1() == 2 || getWonBattlesPlayer2() == 2)
                {
                    nextRound();
                }
                nextRound();
            }
            return GameState.P1Turn;
        }

        private static LogicEngine getDBInstance()
        {
            Table<LogicEngine> LogicEngineTable = Program.db.GetTable<LogicEngine>();

            try
            {
                return LogicEngineTable.First();
            } catch (Exception e)
            {
                return null;
            }
            
        }

        /* Implement this method?
        public static void getDrawResources(out int playedBattlesOUT, out int wonBattlesOUT, out bool winOUT)
        {
            
        }
        */

        public static void cleanDatabase()
        {
            Program.db.ExecuteCommand("DELETE FROM Card");
            Program.db.ExecuteCommand("DELETE FROM CardHandler");
            Program.db.ExecuteCommand("DELETE FROM Player");
            Program.db.ExecuteCommand("DELETE FROM LogicEngine");
        }
        
        public static void generateDatabase(int gameMode)
        {
            Program.db.ExecuteCommand("INSERT INTO LogicEngine VALUES(DEFAULT, DEFAULT, DEFAULT, DEFAULT)");

            Program.db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1},{2},{3})", "player1", 0, 0, 0);
            
            int player1Id = getPlayer1().id;

            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "deck", player1Id);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "hand", player1Id);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "playedCards", player1Id);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "usedCards", player1Id);
            
            int player1DeckId = getPlayer1().getDeck().id;

            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Dragon", "Summons tornados", "dragon.png", 12, player1DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Champion", "Waaaaagh!!!", "warrior_orc.png", 12, player1DeckId);

            // AI/Player2 stuff.....
            if(gameMode == 0)
            {
              Program.db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1},{2},{3})", "player2", gameMode, 0, 0);
            } else
            {
                Program.db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1},{2},{3})", "AI", gameMode, 0, 0);
            }
            
            
            
            int player2Id = getPlayer2().id;

            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "deck", player2Id);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "hand", player2Id);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "playedCards", player2Id);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "usedCards", player2Id);
            
            int player2DeckId = getPlayer2().getDeck().id;

            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 5, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Dragon", "Summons tornados", "dragon.png", 12, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Champion", "Waaaaagh!!", "warrior_orc.png", 9, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 6, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 5, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 12, player2DeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 5, player2DeckId);
            
        }

        public void updateGamestate(GameState state)
        {
            Program.db.ExecuteCommand("UPDATE LogicEngine SET state ={0} WHERE id = {1}", (int)state, this.id);
            Program.db.Refresh(RefreshMode.OverwriteCurrentValues, Program.db.GetTable<LogicEngine>());
        }
    }

    enum GameState
    {
        Start = 1,
        P1Turn = 2,
        P2Turn = 3,
        EndTurn = 4,
        EndGame = 5,
        Problem = -1
    }
}
