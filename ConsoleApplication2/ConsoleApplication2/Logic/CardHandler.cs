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
        [Column(IsPrimaryKey = true)]
        public int id;
        [Column]
        public string type;
        [Column]
        public int playerId;

        public CardHandler()
        {
        }

        
        public Card viewCard(int i)
        {
            return getCards()[i];
        }

        public int numberOFCards()
        {
            return getCards().Count;
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
            Program.db.Refresh(RefreshMode.OverwriteCurrentValues, Program.db.GetTable<Card>());
        }

    }
}
