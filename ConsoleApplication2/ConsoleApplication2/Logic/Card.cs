using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    abstract class Card
    {
        private string description;
        private string image;
        private string name;

        protected Card(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Image = "";
        }

        protected string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        protected string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        protected string Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
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
