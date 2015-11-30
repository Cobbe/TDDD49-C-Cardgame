using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class Player
    {
        protected Hand hand;
        protected Deck deck;
        protected PlayedCards playedCards;
        protected UsedCards usedCards;

        public Player()
        {
            deck = new Deck();
            hand = new Hand();
            playedCards = new PlayedCards();
            usedCards = new UsedCards();
            
        }
    }

    class AI : Player
    {
        public AI() : base()
        {

        }
    }
}
