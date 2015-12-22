using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.GUI
{
    class CardClickbox
    {
        private Logic.Card card;
        private float x, y, width, height;

        public CardClickbox(float x, float y, float width, float height, Logic.Card cardNumber)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.CardNumber = cardNumber;
        }

        public Logic.Card CardNumber
        {
            get
            {
                return card;
            }

            private set
            {
                card = value;
            }
        }

        public bool inBox(int xIn, int yIn)
        {
            return (xIn >= x & xIn <= x + width & yIn >= y & yIn <= y + height);
        }
    }
}
