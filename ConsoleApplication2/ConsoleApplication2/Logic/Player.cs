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
    }
}
