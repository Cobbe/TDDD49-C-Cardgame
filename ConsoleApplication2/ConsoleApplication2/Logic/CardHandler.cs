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
    
}
