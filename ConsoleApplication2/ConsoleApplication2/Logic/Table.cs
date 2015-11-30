using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    class Table
    {
        protected Player player;
        protected AI ai;

        public Table()
        {
            player = new Player();
            ai = new AI();
        }
    }
}
