using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Table
    {
        Player player;
        AI ai;

        public Table()
        {
            player = new Player();
            ai = new AI();
        }
    }
}
