using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class Player
    {
        private Hand hand;
        private Deck deck;
        private PlayedCards playedCards;
        private UsedCards usedCards;
        private int strength;

        public Deck Deck
        {
            get
            {
                return deck;
            }

            set
            {
                deck = value;
            }
        }

        public Hand Hand
        {
            get
            {
                return hand;
            }

            set
            {
                hand = value;
            }
        }

        public PlayedCards PlayedCards
        {
            get
            {
                return playedCards;
            }

            set
            {
                playedCards = value;
            }
        }

        public UsedCards UsedCards
        {
            get
            {
                return usedCards;
            }

            set
            {
                usedCards = value;
            }
        }

        public int Strength
        {
            get
            {
                return strength;
            }

            set
            {
                strength = value;
            }
        }

        public void drawCards(int number)
        {
            for (int i = 0; i < number; i++)
            {
                Hand.addCard(Deck.drawCard());
            }
        }

        public void playCard(Card card)
        {
            if (card is MonsterCard)
            {
                MonsterCard monsterCard = (MonsterCard)card;
                Strength += monsterCard.Strength;
                PlayedCards.addCard(card);

            }
            else
            {
                SpecialCard specialCard = (SpecialCard)card;
                PlayedCards.addCard(card);
            }

        }

        public Player()
        {
            Deck = new Deck();
            Hand = new Hand();
            PlayedCards = new PlayedCards();
            UsedCards = new UsedCards();
            
        }
    }

    class AI : Player
    {
        public AI() : base()
        {

        }

        /* Retrieves the index of the monster card with the highest strength, if there are no monster cards in the deck it sends back -1 */
        public int getStrongestCard()
        {
            int indexOfHigh = -1;
            int strongest = -1;
            for (int i = 0; i < Hand.numberOFCards(); i++)
            {
                if (Hand.viewCard(i) is MonsterCard)
                {
                    int testCardStrength = ((MonsterCard)Hand.viewCard(i)).Strength;
                    if (testCardStrength > strongest)
                    {
                        strongest = testCardStrength;
                        indexOfHigh = i;
                    }
                }
            }
            return indexOfHigh;
        }
    }
}
