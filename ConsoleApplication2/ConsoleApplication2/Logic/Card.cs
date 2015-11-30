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

        protected Card(string name, string description)
        {
            this.name = name;
            this.description = description;
            this.image = "";
        }

    }

    class MonsterCard : Card
    {
        protected int strength;

        public MonsterCard(string name, string description, int strength) : base(name, description)
        {
            this.strength = strength;
        }
    }

    class SpecialCard : Card
    {
        
        public SpecialCard(string name, string description) : base(name, description)
        {

        }
    }
}
