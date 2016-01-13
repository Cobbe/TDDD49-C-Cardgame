using GwentStandalone.LINQ;
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
    public class CardHandler
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
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
            return Storage.getCards(this.id);
        }

        public void moveCardHere(Card card)
        {
            Storage.moveCardToCardHandler(this.id, card);
        }

    }
}
