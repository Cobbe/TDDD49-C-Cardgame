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

        protected Deck Deck
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

        protected Hand Hand
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

        protected PlayedCards PlayedCards
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

        protected UsedCards UsedCards
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
