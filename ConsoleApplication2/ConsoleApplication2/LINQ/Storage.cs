using Logic;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentStandalone.LINQ
{
    public class Storage
    {
        private static Storage storage;
        private static DataContext db = new DataContext(@"Data Source=(localdb)\mssqllocaldb;
                                   Integrated Security=true;
                                   AttachDbFileName=" + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\LINQ\\northwind.mdf");

        private Storage()
        {
            // Setup LINQ
            // DataContext takes a connection string 
            //db = new DataContext(@"Data Source=(localdb)\mssqllocaldb;
             //                      Integrated Security=true;
              //                     AttachDbFileName=" + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\LINQ\\northwind.mdf");
        }

        public static Storage getInstance()
        {
            if (storage == null)
            {
                return storage = new Storage();
            }
            return storage;
        }

        public static Table<Player> getPlayers()
        {
            Table<Player> players = db.GetTable<Player>();

            return players;
        }

        public static Player getPlayer1()
        {
            Table<Player> players = db.GetTable<Player>();

            return players.First();
        }

        public static Player getPlayer2()
        {
            Table<Player> players = db.GetTable<Player>();
            var query = from player in players select player;
            return query.OrderByDescending(player => player.id).First();
        }

        public static CardHandler getCardHandler(int playerId, String type)
        {
            Table<CardHandler> cardHandlers = db.GetTable<CardHandler>();
            var query = from cardHandler in cardHandlers
                        where cardHandler.type == type &&
                        cardHandler.playerId == playerId
                        select cardHandler;
            CardHandler res = null;
            foreach (var cardHandler in query)
                res = cardHandler;

            return res;
        }

        public static void setPass(int playerID, bool pass)
        {
            db.ExecuteCommand("UPDATE Player SET pass ={0} WHERE id = {1}", pass, playerID);
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<Player>());
        }

        public static void setStrength(int playerID, int strength)
        {
            db.ExecuteCommand("UPDATE Player SET strength ={0} WHERE id = {1}", strength, playerID);
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<Player>());
        }

        public static List<Card> getCards(int cardHandlerId)
        {
            Table<Card> cards = db.GetTable<Card>();

            List<Card> filteredCards = (from card in cards
                                        where card.cardHandlerId == cardHandlerId
                                        select card).ToList();

            return filteredCards;
        }

        public static void moveCardToCardHandler(int cardHandlerId, Card card)
        {
            db.ExecuteCommand("UPDATE Card SET cardHandlerId ={0} WHERE id = {1}", cardHandlerId, card.id);
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<Card>());
        }

        public static void updateDB()
        {
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<Player>());
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<Card>());
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<CardHandler>());
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<LogicEngine>());
        }

        public static void cleanDatabase()
        {
            db.ExecuteCommand("DELETE FROM Card");
            db.ExecuteCommand("DELETE FROM CardHandler");
            db.ExecuteCommand("DELETE FROM Player");
            db.ExecuteCommand("DELETE FROM LogicEngine");
        }

        public static void generateDatabase(int gameMode)
        {



            db.ExecuteCommand("INSERT INTO LogicEngine VALUES(DEFAULT, DEFAULT, DEFAULT, DEFAULT)");

            db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1},{2},{3})", "player1", 0, 0, 0);

            int player1Id = getPlayer1().id;

            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "deck", player1Id);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "hand", player1Id);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "playedCards", player1Id);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "usedCards", player1Id);

            int player1DeckId = getPlayer1().getDeck().id;

            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Dragon", "Summons tornados", "dragon.png", 12, player1DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Strong Orc", "Waaaaagh!!!", "warrior_orc.png", 12, player1DeckId);

            Console.WriteLine("Deck: " + player1DeckId);

            db.GetTable<Card>().InsertOnSubmit(new Card("Cobbe", "Fixes Problems", "dragon.png", 99, player1DeckId));
            db.SubmitChanges();

            // AI/Player2 stuff.....
            if (gameMode == 0)
            {
                db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1},{2},{3})", "player2", gameMode, 0, 0);
            }
            else
            {
                db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1},{2},{3})", "AI", gameMode, 0, 0);
            }

            int player2Id = getPlayer2().id;

            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "deck", player2Id);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "hand", player2Id);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "playedCards", player2Id);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "usedCards", player2Id);

            int player2DeckId = getPlayer2().getDeck().id;

            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Strong Orc", "Waaagh!!!", "warrior_orc.png", 5, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Dragon", "Summons tornados", "dragon.png", 12, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Strong Orc", "Waaaaagh!!", "warrior_orc.png", 9, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 6, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Strong Orc", "Waaagh!!!", "warrior_orc.png", 5, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Strong Orc", "Waaagh!!!", "warrior_orc.png", 12, player2DeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 5, player2DeckId);

        }

        public static void updateGamestate(int logicEngineId, GameState state)
        {
            db.ExecuteCommand("UPDATE LogicEngine SET state ={0} WHERE id = {1}", (int)state, logicEngineId);
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<LogicEngine>());
        }

        public static LogicEngine getLogicEngine()
        {
            Table<LogicEngine> LogicEngineTable = db.GetTable<LogicEngine>();

            try
            {
                return LogicEngineTable.First();
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public static void incresePlayerWon(int playerId)
        {
            if (playerId == getPlayer1().id)
            {
                db.ExecuteCommand("UPDATE LogicEngine SET wonBattlesPlayer1 ={0} WHERE id = {1}", getLogicEngine().wonBattlesPlayer1 + 1, getLogicEngine().id);
                db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<LogicEngine>());
            }else if (playerId == getPlayer2().id)
            {
                db.ExecuteCommand("UPDATE LogicEngine SET wonBattlesPlayer2 ={0} WHERE id = {1}", getLogicEngine().wonBattlesPlayer2 + 1, getLogicEngine().id);
                db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<LogicEngine>());
            }

        }
        
        public static void newRound()
        {
            db.ExecuteCommand("UPDATE LogicEngine SET round ={0} WHERE id = {1}", getLogicEngine().round + 1, getLogicEngine().id);
            db.Refresh(RefreshMode.OverwriteCurrentValues, db.GetTable<LogicEngine>());
        }
    }

    
}
