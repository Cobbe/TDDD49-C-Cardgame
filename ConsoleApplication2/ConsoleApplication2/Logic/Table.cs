using GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace Logic
{
    class Table
    {
        private static Table table;
        private Player player;
        private AI ai;
        
        private int cardToPlay;
        private bool win = false;
        private Timer timer;
        private BackgroundWorker gameWorker, graphicsWorker;
        private int waitBetweenActions = 500; // In milliseconds
        private bool firstTurn;
        private int playedBattles, wonBattles;

        private static Random tableRng = new Random();

        public Player Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        protected AI Ai
        {
            get
            {
                return ai;
            }

            set
            {
                ai = value;
            }
        }

        protected int CardToPlay
        {
            get
            {
                return cardToPlay;
            }

            set
            {
                cardToPlay = value;
            }
        }

        protected bool Win
        {
            get
            {
                return win;
            }

            set
            {
                win = value;
            }
        }

        public Timer Timer
        {
            get
            {
                return timer;
            }

            set
            {
                timer = value;
            }
        }

        public int PlayedBattles
        {
            get
            {
                return playedBattles;
            }

            set
            {
                playedBattles = value;
            }
        }

        public int WonBattles
        {
            get
            {
                return wonBattles;
            }

            set
            {
                wonBattles = value;
            }
        }

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
            Timer = new Timer(100);
            Timer.Elapsed += timer_Elapsed;
            Timer.AutoReset = true;
            Timer.Enabled = false;

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

        public void runGame(object Object, DoWorkEventArgs e)
        {
            /*
            XElement cards =
               new XElement("Cards",
                  from c in table.Player.Deck.List
                  select new XElement("card",
                     new XElement("name", c.Name)
                  )
              );

            XElement cards2 =
               new XElement("Cards",
                  from c in table.Player.Hand.List
                  select new XElement("card",
                     new XElement("name", c.Name)
                  )
              );

            XElement cards3 =
               new XElement("Cards",
                  from c in table.Player.PlayedCards.List
                  select new XElement("card",
                     new XElement("name", c.Name)
                  )
              );

            Console.WriteLine(cards);
            Console.WriteLine(cards2);
            Console.WriteLine(cards3);
            */

            if (PlayedBattles < 3)
            {
                if ((!player.Pass || !ai.Pass) && ((player.Hand.numberOFCards() > 0 || ai.Hand.numberOFCards() > 0) || firstTurn))
                {
                    // Turn-based

                    // Start by drawing cards
                    if (firstTurn)
                    {
                        player.drawCards(10);
                        //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        //System.Threading.Thread.Sleep(waitBetweenActions);

                        ai.drawCards(10);
                        //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        //System.Threading.Thread.Sleep(waitBetweenActions);
                        firstTurn = false;

                    }
                    // Play card
                    if(player.Hand.numberOFCards() > 0 && !player.Pass)
                    {
                        while (!GameForm.getGameForm().ActiveClick)
                        {
                            if (player.Pass)
                            {
                                break;
                            }
                        }
                        if (!player.Pass)
                        {
                            player.playCard(player.Hand.getCard(GameForm.getGameForm().LastClickedBox));
                            GameForm.getGameForm().ActiveClick = false;
                            //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                            //System.Threading.Thread.Sleep(waitBetweenActions);
                        }
                        
                    }

                    // Play card
                    if (ai.Hand.numberOFCards() > 0 && !ai.Pass)
                    {
                        ai.determineAndPerformAction(player.Strength, PlayedBattles+1, PlayedBattles - WonBattles, player.Pass);
                        //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        //System.Threading.Thread.Sleep(waitBetweenActions);
                    } else
                    {
                        ai.Pass = true;
                    }
                }
                else
                {
                    if (player.Strength > ai.Strength)
                    {
                        wonBattles++;
                    }
                    playedBattles++;
                    ai.Pass = false;
                    player.Pass = false;
                    player.PlayedCards.clear();
                    ai.PlayedCards.clear();
                    player.Strength = 0;
                    ai.Strength = 0;
                    //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                }
            } else
            {
                if (WonBattles >=2 )
                {
                    win = true;
                }
                else
                {
                    win = false;
                }
                PlayedBattles++;
                //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
            }
        }

        protected void initializeGame()
        {
            Player = new Player();
            Ai = new AI();
            initialDecks();
            playedBattles = 0;
            wonBattles = 0;
            player.Pass = false;
            ai.Pass = false;
            firstTurn = true;
        }

        public Table<Player> getPlayers(DataContext db)
        {
            Table<Player> players = db.GetTable<Player>();

            return players;
        }
        
        protected void initialDecks()
        {
            // For player
            player.Deck.clear();
            player.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Wind Drake", "Has sharp claws", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Wind Drake", "Has sharp claws", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Wind Drake", "Has sharp claws", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Wind Drake", "Has sharp claws", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            player.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            player.Deck.addCard(new MonsterCard("Orc Champion", "Waaaaagh!!!", "warrior_orc.png", 12));
            player.Deck.shuffle();

            // For AI
            ai.Deck.clear();
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 5));
            ai.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            ai.Deck.addCard(new MonsterCard("Orc Champion", "Waaaaagh!!", "warrior_orc.png", 9));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 6));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 5));
            ai.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 12));
            ai.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 5));
            ai.Deck.shuffle();
        }

        public static void getDrawResources(out Player playerOUT, out AI aiOUT, out int playedBattlesOUT, out int wonBattlesOUT, out bool winOUT)
        {
            playerOUT = table.player;
            aiOUT = table.ai;
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
