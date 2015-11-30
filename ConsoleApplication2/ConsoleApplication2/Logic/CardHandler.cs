using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    abstract class CardHandler
    {
        protected List<Card> list;

        protected CardHandler()
        {
            list = new List<Card>();
        }

        public void addCard(Card c)
        {
            list.Add(c);
        }

        public Card getCard(int i)
        {
            Card c = list[i];
            list.RemoveAt(i);
            return c;
        }

        public Card viewCard(int i)
        {
            return list[i];
        }

        public int numberOFCards()
        {
            return list.Count;
        }
    }

    class Deck : CardHandler
    {
        private static Random rng = new Random();

        public Deck() : base()
        {

        }

        public Card drawCard()
        {
            return getCard(numberOFCards() - 1);
        }

        public void shuffle(List<Card> deck)
        {
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        // https://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
        }
    }

    class Hand : CardHandler
    {
        public Hand() : base()
        {

        }
    }

    class PlayedCards : CardHandler
    {
        public PlayedCards() : base()
        {

        }
        
    }

    class UsedCards : Deck
    {
        public UsedCards() : base()
        {

        }

        void consume(PlayedCards played)
        {
            while(played.numberOFCards() > 0)
            {
                addCard(played.getCard(0));
            }
        }
    }
}
