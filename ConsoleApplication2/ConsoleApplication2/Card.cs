using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Card
    {
        public String name, description;
        public String image;

        protected Card(String name, String description)
        {
            this.name = name;
            this.description = description;
            this.image = "";
        }

    }

    class MonsterCard : Card
    {
        int strength;

        public MonsterCard(String name, String description, int strength) : base(name, description)
        {
            this.strength = strength;
        }
    }

    class SpecialCard : Card
    {
        
        public SpecialCard(String name, String description) : base(name, description)
        {

        }
    }
}
