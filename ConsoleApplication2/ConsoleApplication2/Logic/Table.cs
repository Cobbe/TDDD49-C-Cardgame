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
        private static Player player;
        private static AI ai;
        
        private int turn;
        private int cardToPlay;
        private MonsterCard playMonster;
        private SpecialCard playSpecial;
        private Timer timer;

        private static Random tableRng = new Random();

        protected static Player Player
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

        protected static AI Ai
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

            /*
            timer = new System.Timers.Timer(13);
            timer.Elapsed += runGame;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            */
        }

        public void runGame()
        {
            if (turn < 5)
            {
                // Turn-based
                Console.WriteLine("Turn: " + turn);

                // Player Turn
                player.Hand.addCard(player.Deck.drawCard());
                player.Hand.addCard(player.Deck.drawCard());
                for (int i = 0; i < player.Hand.numberOFCards(); i++)
                {
                    Console.WriteLine("Card #" + (i + 1) + " : " + player.Hand.viewCard(i).Name);
                }

                //do
                //{
                //    Console.WriteLine("Enter the number of the card you wish to play, from left to right, starting at 1: ");
                //    cardToPlay = Convert.ToInt32(Console.ReadLine());
                //    Console.WriteLine("");
                //} while (cardToPlay <= 0 && cardToPlay > player.Hand.numberOFCards());

                playCard(player, player.Hand.getCard(tableRng.Next() % player.Hand.numberOFCards()));

                // AI Turn
                ai.Hand.addCard(ai.Deck.drawCard());
                ai.Hand.addCard(ai.Deck.drawCard());
                cardToPlay = tableRng.Next() % ai.Hand.numberOFCards();
                playCard(ai, ai.Hand.getCard(cardToPlay));

                Console.WriteLine("Player has " + player.Strength + " Strength : AI has " + ai.Strength + " Strength");
                turn++;
            }

            if (turn == 5)
            {
                if (player.Strength > ai.Strength)
                {
                    Console.WriteLine("Victory!");
                }
                else
                {
                    Console.WriteLine("Defeat!");
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
            player.Deck.addCard(new MonsterCard("Ice Dragon", "Freezes anything it bites", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            player.Deck.addCard(new MonsterCard("Earth Dragon", "Has very tough skin", "dragon.png", 6));
            player.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            player.Deck.addCard(new MonsterCard("Ice Dragon", "Freezes anything it bites", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            player.Deck.addCard(new MonsterCard("Earth Dragon", "Has very tough skin", "dragon.png", 6));
            player.Deck.shuffle();

            // For AI
            ai.Deck.clear();
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Ice Dragon", "Freezes anything it bites", "dragon.png", 8));
            ai.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            ai.Deck.addCard(new MonsterCard("Earth Dragon", "Has very tough skin", "dragon.png", 6));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Ice Dragon", "Freezes anything it bites", "dragon.png", 8));
            ai.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            ai.Deck.addCard(new MonsterCard("Earth Dragon", "Has very tough skin", "dragon.png", 6));
            ai.Deck.shuffle();
        }

        protected void playCard(Player player, Card card)
        {
            if (card is MonsterCard)
            {
                PlayMonster = (MonsterCard)card;
                player.Strength += PlayMonster.Strength;

            }
            else
            {
                PlaySpecial = (SpecialCard)card;
            }

        }

        protected void drawCards()
        {

        }

        public static void getDrawResources(out Player playerOUT, out AI aiOUT)
        {
            playerOUT = player;
            aiOUT = ai;
        }
        public static void tick()
        {
            table.runGame();
        }
    }
}
