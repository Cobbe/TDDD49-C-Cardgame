using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class Table
    {
        private Player player;
        private AI ai;
        private int turn;
        private int cardToPlay;
        private MonsterCard playMonster;
        private SpecialCard playSpecial;

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

        public Table()
        {
            Player = new Player();
            Ai = new AI();
        }

        public void runGame()
        {
            initializeGame();
            while (true)
            {
                // Turn-based
                Console.WriteLine("Turn: " + turn);

                // Player Turn
                player.Hand.addCard(player.Deck.drawCard());
                player.Hand.addCard(player.Deck.drawCard());
                for (int i = 0; i<player.Hand.numberOFCards(); i++)
                {
                    Console.WriteLine("Card #"+(i+1)+" : "+player.Hand.viewCard(i).Name);
                }
                Console.WriteLine("Enter the number of the card you wish to play, from left to right, starting at 1: ");
                cardToPlay = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("");
                while(cardToPlay <= 0 && cardToPlay > player.Hand.numberOFCards())
                {
                    Console.WriteLine("Bad input");
                    Console.WriteLine("Enter the number of the card you wish to play, from left to right, starting at 1: ");
                    cardToPlay = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("");
                }

                playCard(player, player.Hand.getCard(cardToPlay-1));

                // AI Turn
                ai.Hand.addCard(ai.Deck.drawCard());
                ai.Hand.addCard(ai.Deck.drawCard());
                cardToPlay = tableRng.Next() % ai.Hand.numberOFCards();
                playCard(ai, ai.Hand.getCard(cardToPlay));

                Console.WriteLine("Player has " + player.Strength + " Strength : AI has " + ai.Strength + " Strength");
                turn++;
                if(turn == 3)
                {
                    break;
                }
            }
            if(player.Strength > ai.Strength)
            {
                Console.WriteLine("Victory!");
            } else
            {
                Console.WriteLine("Defeat!");
            }
        }
        protected void initializeGame()
        {
            turn = 1;
            initialDecks();
            
        }
        
        protected void initialDecks()
        {
            // For player
            player.Deck.clear();
            player.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "Dragon.jpg", 10));
            player.Deck.addCard(new MonsterCard("Ice Dragon", "Freezes anything it bites", "Dragon.jpg", 8));
            player.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "Dragon.jpg", 12));
            player.Deck.addCard(new MonsterCard("Earth Dragon", "Has very tough skin", "Dragon.jpg", 6));
            player.Deck.shuffle();

            // For AI
            ai.Deck.clear();
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "Dragon.jpg", 10));
            ai.Deck.addCard(new MonsterCard("Ice Dragon", "Freezes anything it bites", "Dragon.jpg", 8));
            ai.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "Dragon.jpg", 12));
            ai.Deck.addCard(new MonsterCard("Earth Dragon", "Has very tough skin", "Dragon.jpg", 6));
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

        protected void
    }
}
