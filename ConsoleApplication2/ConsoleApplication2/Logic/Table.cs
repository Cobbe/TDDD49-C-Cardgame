using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Logic
{
    class Table
    {
        private static Table table;
        private Player player;
        private AI ai;
        
        private int turn;
        private int cardToPlay;
        private MonsterCard playMonster;
        private SpecialCard playSpecial;
        private bool win = false;
        private Timer timer;

        private static Random tableRng = new Random();

        protected Player Player
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

        protected int Turn
        {
            get
            {
                return turn;
            }

            set
            {
                turn = value;
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

        internal SpecialCard PlaySpecial
        {
            get
            {
                return playSpecial;
            }

            set
            {
                playSpecial = value;
            }
        }

        internal MonsterCard PlayMonster
        {
            get
            {
                return playMonster;
            }

            set
            {
                playMonster = value;
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

        public static Table createTableInstance()
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

            
            Timer = new Timer(2000);
            Timer.Elapsed += runGame;
            Timer.AutoReset = true;
            Timer.Enabled = false;
            
        }

        public void runGame(object Object, ElapsedEventArgs e)
        {
            if (turn < 6)
            {
                // Turn-based

                // Player Turn

                // Start by drawing cards
                drawCards(Player, 2);
                ConsoleApplication2.GameForm.getGameForm().updateGraphics();

                /*
                for (int i = 0; i < player.Hand.numberOFCards(); i++)
                {
                    Console.WriteLine("Card #" + (i + 1) + " : " + player.Hand.viewCard(i).Name);
                }
                */

                //do
                //{
                //    Console.WriteLine("Enter the number of the card you wish to play, from left to right, starting at 1: ");
                //    cardToPlay = Convert.ToInt32(Console.ReadLine());
                //    Console.WriteLine("");
                //} while (cardToPlay <= 0 && cardToPlay > player.Hand.numberOFCards());

                playCard(player, player.Hand.getCard(playStrongestCard(player)));
                ConsoleApplication2.GameForm.getGameForm().updateGraphics();

                // AI Turn
                // Start by drawing cards
                drawCards(Ai, 2);
                ConsoleApplication2.GameForm.getGameForm().updateGraphics();

                playCard(ai, ai.Hand.getCard(playStrongestCard(ai)));
                ConsoleApplication2.GameForm.getGameForm().updateGraphics();

                turn++;
            } else
                {
                turn++;
                    if (player.Strength > ai.Strength)
                    {
                        win = true;
                        ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                    }
                    else
                    {
                        win = false;
                        ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                    }
                }
        }
        protected void initializeGame()
        {
            Player = new Player();
            Ai = new AI();
            turn = 1;
            initialDecks();
            
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
            player.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            player.Deck.shuffle();

            // For AI
            ai.Deck.clear();
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 5));
            ai.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 5));
            ai.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 5));
            ai.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            ai.Deck.shuffle();
        }

        protected void playCard(Player player, Card card)
        {
            if (card is MonsterCard)
            {
                PlayMonster = (MonsterCard)card;
                player.Strength += PlayMonster.Strength;
                player.PlayedCards.addCard(card);

            }
            else
            {
                PlaySpecial = (SpecialCard)card;
                player.PlayedCards.addCard(card);
            }

        }

        protected void drawCards(Player player, int number)
        {
            for(int i = 0; i < number; i++)
            {
                player.Hand.addCard(player.Deck.drawCard());
            }
        }

        protected int playStrongestCard(Player player)
        {
            int indexOfHigh = 0;
            for(int i = 1; i<player.Hand.numberOFCards(); i++)
            {
                if(player.Hand.viewCard(indexOfHigh) is MonsterCard)
                {
                    if(player.Hand.viewCard(i) is MonsterCard)
                    {
                        MonsterCard temp1 = (MonsterCard)player.Hand.viewCard(indexOfHigh);
                        MonsterCard temp2 = (MonsterCard)player.Hand.viewCard(i);
                        if(temp2.Strength > temp1.Strength)
                        {
                            indexOfHigh = i;
                        }
                    }
                }
            }
            return indexOfHigh;
        }

        public static void getDrawResources(out Player playerOUT, out AI aiOUT, out int turnOUT, out bool winOUT)
        {
            playerOUT = table.player;
            aiOUT = table.ai;
            turnOUT = table.turn;
            winOUT = table.win;
        }

        /*
        public static void tick()
        {
            table.runGame();
        }
        */
    }
}
