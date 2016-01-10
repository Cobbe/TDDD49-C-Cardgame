using GUI;
using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Timers;

namespace Logic
{
    class Table
    {
        private static Table table;
        
        private int cardToPlay;
        private bool win = false;
        private Timer timer;
        private BackgroundWorker gameWorker, graphicsWorker;
        private int waitBetweenActions = 500; // In milliseconds
        private bool firstTurn;
        private int playedBattles, wonBattles;

        private static Random tableRng = new Random();

        public static Table getTableInstance(DataContext db)
        {
            if(table != null)
            {
                return table;
            } else
            {
                return table = new Table(db);
            }
        }

        private Table(DataContext db)
        {
            initializeGame(db);

            gameWorker = new BackgroundWorker();
            gameWorker.DoWork += runGame;
            timer = new Timer(100);
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = false;

            graphicsWorker = new BackgroundWorker();
            graphicsWorker.DoWork += GameForm.getGameForm().updateGraphics;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!gameWorker.IsBusy)
            {
                if (!graphicsWorker.IsBusy)
                {
                    graphicsWorker.RunWorkerAsync();
                }
                gameWorker.RunWorkerAsync();
            }
        }

        public void runGame(object Object, DoWorkEventArgs e, DataContext db)
        {
            Player player = getPlayer("player", db);
            Player ai = getPlayer("player", db);

            if (playedBattles < 3)
            {
                if ((!player.pass || !ai.pass) && ((player.getHand(db).numberOFCards() > 0 || ai.getHand(db).numberOFCards() > 0) || firstTurn))
                {
                    // Turn-based

                    // Start by drawing cards
                    if (firstTurn)
                    {
                        player.drawCards(10, db);
                        //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        //System.Threading.Thread.Sleep(waitBetweenActions);

                        ai.drawCards(10, db);
                        //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        //System.Threading.Thread.Sleep(waitBetweenActions);
                        firstTurn = false;

                    }
                    // Play card
                    if(player.getHand(db).numberOFCards() > 0 && !player.pass)
                    {
                        while (!GameForm.getGameForm().ActiveClick)
                        {
                            if (player.pass)
                            {
                                break;
                            }
                        }
                        if (!player.pass)
                        {
                            player.playCard(player.getHand(db).getCard(GameForm.getGameForm().LastClickedBox));
                            GameForm.getGameForm().ActiveClick = false;
                            //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                            //System.Threading.Thread.Sleep(waitBetweenActions);
                        }
                        
                    }

                    // Play card
                    if (ai.getHand(db).numberOFCards() > 0 && !ai.pass)
                    {
                        ai.determineAndPerformAction(player.strength, playedBattles+1, playedBattles - wonBattles, player.pass, db);
                        //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        //System.Threading.Thread.Sleep(waitBetweenActions);
                    } else
                    {
                        ai.pass = true;
                    }
                }
                else
                {
                    if (player.strength > ai.strength)
                    {
                        wonBattles++;
                    }
                    playedBattles++;
                    ai.pass = false;
                    player.pass = false;
                    player.strength = 0;
                    ai.strength = 0;
                    //CLEAR CARDS
                }
            } else
            {
                if (wonBattles >=2 )
                {
                    win = true;
                }
                else
                {
                    win = false;
                }
                playedBattles++;
                //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
            }
        }

        protected void initializeGame(DataContext db)
        {
            playedBattles = 0;
            wonBattles = 0;
            getPlayer("player", db).pass = false;
            getPlayer("ai", db).pass = false;
            firstTurn = true;
        }

        public Table<Player> getPlayers(DataContext db)
        {
            Table<Player> players = db.GetTable<Player>();

            return players;
        }
        
        public Player getPlayer(string name, DataContext db)
        {
            var query = from player in getPlayers(db)
                        where player.name == name
                        select player;
            Player res = null;

            foreach (var player in query)
                res = player;

            return res;
        }
        
        public static void getDrawResources(out Player playerOUT, out Player aiOUT, out int playedBattlesOUT, out int wonBattlesOUT, out bool winOUT, DataContext db)
        {
            playerOUT = table.getPlayer("player", db);
            aiOUT = table.getPlayer("ai", db);
            playedBattlesOUT = table.playedBattles;
            wonBattlesOUT = table.wonBattles;
            winOUT = table.win;
        }

        public void cleanDatabase(DataContext db)
        {
            int i;

            i = db.ExecuteCommand("DELETE FROM Card");
            Console.WriteLine("TEST01: " + i);
            i = db.ExecuteCommand("DELETE FROM CardHandler");
            Console.WriteLine("TEST02: " + i);
            i = db.ExecuteCommand("DELETE FROM Player");
            Console.WriteLine("TEST03: " + i);
        }
        
        public void generateDatabase(DataContext db)
        {
            db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1})", "player", 0);
            
            Table<Player> players = db.GetTable<Player>();
            var query = from player in players
                        where player.name == "player"
                        select player;
            int playerId = -1;
            foreach (var player in query)
                playerId = player.id;

            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "deck", playerId);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "hand", playerId);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "playedCards", playerId);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "usedCards", playerId);

            Table<CardHandler> cardHandlers = db.GetTable<CardHandler>();

            var query2 = from cardHandler in cardHandlers
                    where cardHandler.type == "deck" && 
                    cardHandler.playerId == playerId
                    select cardHandler;

            int playerDeckId = 0;
            foreach (var cardHandler in query2)
            {
                playerDeckId = cardHandler.id;
            }

            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Dragon", "Summons tornados", "dragon.png", 12, playerDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Champion", "Waaaaagh!!!", "warrior_orc.png", 12, playerDeckId);

            //AI stuff.....
            db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1})", "ai", 1);

            Table<Player> ais = db.GetTable<Player>();
            var aiquery = from player in players
                        where player.name == "ai"
                        select player;
            int aiId = -1;
            foreach (var ai in aiquery)
                aiId = ai.id;

            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "deck", aiId);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "hand", aiId);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "playedCards", aiId);
            db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "usedCards", aiId);

            cardHandlers = db.GetTable<CardHandler>();

            var aiquery2 = from cardHandler in cardHandlers
                         where cardHandler.type == "deck" &&
                         cardHandler.playerId == aiId
                           select cardHandler;

            int aiDeckId = 0;
            foreach (var cardHandler in aiquery2)
            {
                aiDeckId = cardHandler.id;
            }

            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 5, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Dragon", "Summons tornados", "dragon.png", 12, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Champion", "Waaaaagh!!", "warrior_orc.png", 9, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 6, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 5, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 12, aiDeckId);
            db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 5, aiDeckId);

            db.SubmitChanges();
            db.Refresh(RefreshMode.KeepCurrentValues);
            
        }
    }
}
