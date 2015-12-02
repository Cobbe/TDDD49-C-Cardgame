using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    abstract class Card
    {
        protected string name, description;
        protected string image;

        protected Card(string name, string description, string image)
        {
            this.name = name;
            this.description = description;
            this.image = image;
        }

        public String getImage()
        {
            return image;
        }

        public String getName()
        {
            return name;
        }

        public String getDescription()
        {
            return description;
        }
    }

    class MonsterCard : Card
    {
        protected int strength;

        public MonsterCard(string name, string description, string image, int strength) : base(name, description, image)
        {
            this.strength = strength;
        }
    }

    class SpecialCard : Card
    {
        
        public SpecialCard(string name, string description, string image) : base(name, description, image)
        {

        }
    }
}
