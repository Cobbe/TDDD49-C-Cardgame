using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class Table
    {
        private Player player;
        private AI ai;

        protected Player Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        protected AI Ai
        {
            get
            {
                return ai;
            }

            set
            {
                ai = value;
            }
        }

        public Table()
        {
            Player = new Player();
            Ai = new AI();
        }

        public void runGame()
        {
            
            while (true)
            {
                Console.WriteLine("Thread is running");
            }
        }

    }
}
