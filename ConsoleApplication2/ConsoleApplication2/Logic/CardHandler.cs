using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    abstract class CardHandler
    {
        private List<Card> list;

        public List<Card> List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }

        protected CardHandler()
        {
            List = new List<Card>();
        }

        public void addCard(Card c)
        {
            List.Add(c);
        }

        public Card getCard(int i)
        {
            Card c = List[i];
            List.RemoveAt(i);
            return c;
        }

        public Card viewCard(int i)
        {
            return List[i];
        }

        public int numberOFCards()
        {
            return List.Count;
        }

        public void clear()
        {
            List.RemoveRange(0, numberOFCards());
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

        public void shuffle()
        {
            int n = List.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = List[k];
                List[k] = List[n];
                List[n] = value;
            }
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
