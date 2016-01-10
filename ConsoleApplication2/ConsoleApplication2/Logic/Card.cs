using System.Data.Linq.Mapping;

namespace Logic
{
    [Table(Name = "Card")]
    class Card
    {
        [Column(IsPrimaryKey = true)]
        private int id;
        [Column]
        private string description;
        [Column]
        private string image;
        [Column]
        private string name;
        [Column]
        private int strength;
        [Column]
        public int cardHandlerId;

        public Card() : base()
        {

        }

        protected Card(string name, string description, string image)
        {
            this.Name = name;
            this.Description = description;
            this.Image = image;
        }

        public int Id
        {
            get
            {
                return id;
            }

            private set
            {
                id = value;
            }
        }
        
        public string Name
        {
            get
            {
                return name;
            }

            private set
            {
                name = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            private set
            {
                description = value;
            }
        }

        public int Strength
        {
            get
            {
                return strength;
            }

            set
            {
                strength = value;
            }
        }

        public string Image
        {
            get
            {
                return image;
            }

            private set
            {
                image = value;
            }
        }
    }

    class MonsterCard : Card
    {
        

        public MonsterCard(string name, string description, string image, int strength) : base(name, description, image)
        {
            this.Strength = strength;
        }

        
    }

    class SpecialCard : Card
    {
        
        public SpecialCard(string name, string description, string image) : base(name, description, image)
        {

        }
    }
}
