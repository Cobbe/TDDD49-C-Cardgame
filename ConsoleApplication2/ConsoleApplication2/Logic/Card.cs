﻿using System;
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

        protected Card(string name, string description, string image)
        {
            this.Name = name;
            this.Description = description;
            this.Image = image;
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
        private int strength;

        public MonsterCard(string name, string description, string image, int strength) : base(name, description, image)
        {
            this.Strength = strength;
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
    }

    class SpecialCard : Card
    {
        
        public SpecialCard(string name, string description, string image) : base(name, description, image)
        {

        }
    }
}
