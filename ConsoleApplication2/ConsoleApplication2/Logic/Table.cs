using GUI;
using GwentStandAlone;
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
        public Timer timer;
        private BackgroundWorker gameWorker, graphicsWorker;
        private int waitBetweenActions = 500; // In milliseconds
        private bool firstTurn;
        private int playedBattles, wonBattles;

        private static Random tableRng = new Random();

        public static Table getTableInstance()
        {
            if(table != null)
            {
                return table;
            } else
            {
                return table = new Table();
            }
        }

        private Table()
        {
            initializeGame();

            gameWorker = new BackgroundWorker();
            gameWorker.DoWork += runGame;
            timer = new Timer(100);
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = false;

            //graphicsWorker = new BackgroundWorker();
            //graphicsWorker.DoWork += GameForm.getGameForm().updateGraphics;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!gameWorker.IsBusy)
            {
                /*if (!graphicsWorker.IsBusy)
                {
                    graphicsWorker.RunWorkerAsync();
                }*/
                gameWorker.RunWorkerAsync();
            }
        }

        public void runGame(object Object, DoWorkEventArgs e)
        {
            if (playedBattles < 3)
            {
                if ((!getPlayer("player").pass || !getPlayer("ai").pass) && ((getPlayer("player").getHand().numberOFCards() > 0 || getPlayer("ai").getHand().numberOFCards() > 0) || firstTurn))
                {
                    // Turn-based

                    // Start by drawing cards
                    if (firstTurn)
                    {
                        getPlayer("player").drawCards(10);
                        GameForm.getGameForm().updateGraphics();
                        //System.Threading.Thread.Sleep(waitBetweenActions);

                        getPlayer("ai").drawCards(10);
                        GameForm.getGameForm().updateGraphics();
                        //System.Threading.Thread.Sleep(waitBetweenActions);
                        firstTurn = false;

                    }
                    // Play card
                    /*
                    if(getPlayer("player").getHand().numberOFCards() > 0 && !getPlayer("player").pass)
                    {
                        while (!GameForm.getGameForm().ActiveClick)
                        {
                            if (getPlayer("player").pass)
                            {
                                break;
                            }
                        }
                        if (!getPlayer("player").pass)
                        {
                            getPlayer("player").playCard(getPlayer("player").getHand().viewCard(GameForm.getGameForm().LastClickedBox));
                            GameForm.getGameForm().ActiveClick = false;
                            GameForm.getGameForm().updateGraphics();
                            //System.Threading.Thread.Sleep(waitBetweenActions);
                        }
                        
                    }
                    */

                    // Play card
                    if (getPlayer("player").getHand().numberOFCards() > 0 && !getPlayer("player").pass)
                    {
                        getPlayer("player").determineAndPerformAction(getPlayer("ai").strength, playedBattles + 1, wonBattles, getPlayer("ai").pass);
                        GameForm.getGameForm().updateGraphics();
                        System.Threading.Thread.Sleep(waitBetweenActions);
                    }
                    else
                    {
                        getPlayer("player").setPass(true);
                    }

                    // Play card
                    if (getPlayer("ai").getHand().numberOFCards() > 0 && !getPlayer("ai").pass)
                    {
                        getPlayer("ai").determineAndPerformAction(getPlayer("player").strength, playedBattles+1, playedBattles - wonBattles, getPlayer("player").pass);
                        GameForm.getGameForm().updateGraphics();
                        System.Threading.Thread.Sleep(waitBetweenActions);
                    } else
                    {
                        getPlayer("ai").setPass(true);
                    }
                }
                else
                {
                    if (getPlayer("player").strength > getPlayer("ai").strength)
                    {
                        wonBattles++;
                    }
                    playedBattles++;
                    getPlayer("ai").setPass(false);
                    getPlayer("player").setPass(false);
                    getPlayer("player").setStrength(0);
                    getPlayer("ai").setStrength(0);
                    foreach (Card card in getPlayer("player").getPlayedCards().getCards())
                        getPlayer("player").getUsedCards().moveCardHere(card);
                    foreach (Card card in getPlayer("ai").getPlayedCards().getCards())
                        getPlayer("ai").getUsedCards().moveCardHere(card);
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
                GameForm.getGameForm().updateGraphics();
            }
        }

        protected void initializeGame()
        {
            playedBattles = 0;
            wonBattles = 0;
            //getPlayer("player").pass = false;
            //getPlayer("ai").pass = false;
            firstTurn = true;
        }

        public Table<Player> getPlayers()
        {
            Table<Player> players = Program.db.GetTable<Player>();

            return players;
        }
        
        public Player getPlayer(string name)
        {
            Table<Player> players = Program.db.GetTable<Player>();

            var query = from player in players
                        where player.name == name
                        select player;
            return query.First();
        }
        
        public static void getDrawResources(out int playedBattlesOUT, out int wonBattlesOUT, out bool winOUT)
        {
            playedBattlesOUT = table.playedBattles;
            wonBattlesOUT = table.wonBattles;
            winOUT = table.win;
        }

        public void cleanDatabase()
        {
            int i;

            i = Program.db.ExecuteCommand("DELETE FROM Card");
            Console.WriteLine("TEST01: " + i);
            i = Program.db.ExecuteCommand("DELETE FROM CardHandler");
            Console.WriteLine("TEST02: " + i);
            i = Program.db.ExecuteCommand("DELETE FROM Player");
            Console.WriteLine("TEST03: " + i);
        }
        
        public void generateDatabase()
        {
            Program.db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1},{2},{3})", "player", 0, 0, 0);
            
            int playerId = getPlayer("player").id;

            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "deck", playerId);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "hand", playerId);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "playedCards", playerId);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "usedCards", playerId);
            
            int playerDeckId = getPlayer("player").getDeck().id;

            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Drake", "Has sharp claws", "dragon.png", 8, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Dragon", "Summons tornados", "dragon.png", 12, playerDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Champion", "Waaaaagh!!!", "warrior_orc.png", 12, playerDeckId);

            //AI stuff.....
            Program.db.ExecuteCommand("INSERT INTO Player VALUES ({0},{1},{2},{3})", "ai", 1, 0, 0);
            
            int aiId = getPlayer("ai").id;

            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "deck", aiId);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "hand", aiId);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "playedCards", aiId);
            Program.db.ExecuteCommand("INSERT INTO CardHandler VALUES ({0},{1})", "usedCards", aiId);
            
            int aiDeckId = getPlayer("ai").getDeck().id; ;

            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 5, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Wind Dragon", "Summons tornados", "dragon.png", 12, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Champion", "Waaaaagh!!", "warrior_orc.png", 9, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 6, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Fire Dragon", "Breathes fire", "dragon.png", 10, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc", "Waaagh!!", "warrior_orc.png", 2, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 5, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 3, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Orc Commander", "Waaagh!!!", "warrior_orc.png", 12, aiDeckId);
            Program.db.ExecuteCommand("INSERT INTO Card (name, description, image, strength, cardHandlerId)VALUES ({0},{1},{2},{3},{4})", "Witch", "Dark Sorcery", "witch.png", 5, aiDeckId);
            
        }
    }
}
