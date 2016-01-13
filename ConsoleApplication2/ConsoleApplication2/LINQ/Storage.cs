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
            db.GetTable<Card>().DeleteAllOnSubmit(db.GetTable<Card>());
            db.GetTable<CardHandler>().DeleteAllOnSubmit(db.GetTable<CardHandler>());
            db.GetTable<Player>().DeleteAllOnSubmit(db.GetTable<Player>());
            db.GetTable<LogicEngine>().DeleteAllOnSubmit(db.GetTable<LogicEngine>());
            db.SubmitChanges();
        }

        public static void generateDatabase(int gameMode, int deckSize)
        {
            db.GetTable<LogicEngine>().InsertOnSubmit(new LogicEngine());

            bool playerVSAi = gameMode != 0;
            db.GetTable<Player>().InsertOnSubmit(new Player("player1", false));
            db.GetTable<Player>().InsertOnSubmit(new Player(playerVSAi ? "AI" : "player2", playerVSAi));

            db.SubmitChanges();
            
            foreach (int playerId in new int[]{ getPlayer1().id, getPlayer2().id })
            {
                db.GetTable<CardHandler>().InsertOnSubmit(new CardHandler("deck", playerId));
                db.GetTable<CardHandler>().InsertOnSubmit(new CardHandler("hand", playerId));
                db.GetTable<CardHandler>().InsertOnSubmit(new CardHandler("playedCards", playerId));
                db.GetTable<CardHandler>().InsertOnSubmit(new CardHandler("usedCards", playerId));
            }
            db.SubmitChanges();

            int player1DeckId = getPlayer1().getDeck().id;
            int player2DeckId = getPlayer2().getDeck().id;

            CardGenerator generator = new CardGenerator();
            for(int i=0; i<deckSize; i++)
            {
                db.GetTable<Card>().InsertOnSubmit(generator.generateRandomCard(player1DeckId));
                db.GetTable<Card>().InsertOnSubmit(generator.generateRandomCard(player2DeckId));
            }
            
            db.SubmitChanges();
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
