using GwentStandAlone;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    [Table(Name = "CardHandler")]
    class CardHandler
    {
        public List<Card> list;

        [Column(IsPrimaryKey = true)]
        public int id;
        [Column]
        public string type;
        [Column]
        public int playerId;

        public CardHandler()
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

        public void clear()
        {
            list.RemoveRange(0, numberOFCards());
        }

        public List<Card> getCards()
        {
            Table<Card> cards = Program.db.GetTable<Card>();

            List<Card> filteredCards = (from card in cards
                                        where card.cardHandlerId == this.id
                                        select card).ToList();

            return filteredCards;
        }

        public void moveCardHere(Card card)
        {
            Program.db.ExecuteCommand("UPDATE Card SET cardHandlerId ={0} WHERE id = {1}", this.id, card.id);
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
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = list[k];
                list[k] = list[n];
                list[n] = value;
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
