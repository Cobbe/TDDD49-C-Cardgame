using System.Data.Linq.Mapping;

namespace Logic
{
    [Table(Name = "Card")]
    public class Card
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id;
        [Column]
        public string description;
        [Column]
        public string image;
        [Column]
        public string name;
        [Column]
        public int strength;
        [Column]
        public int cardHandlerId;

        public Card() : base()
        {

        }

        public Card(string name, string description, string image, int strength)
        {
            this.name = name;
            this.description = description;
            this.image = image;
            this.strength = strength;
        }

        public Card(string name, string description, string image, int strength, int cardHandlerId) : this(name, description, image, strength)
        {
            this.cardHandlerId = cardHandlerId;
        }
    }
}
