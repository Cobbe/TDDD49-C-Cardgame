using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Player
    {
        Hand hand;
        Deck deck;
        PlayedCards playedCards;
        UsedCards usedCards;

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
