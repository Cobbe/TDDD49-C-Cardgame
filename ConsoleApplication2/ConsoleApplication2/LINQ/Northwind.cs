using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ
{
    public partial class Northwind : DataContext
    {
        public Table<CardHandler> cardHandlers;
        public Table<Order> Orders;
        public Northwind(string connection) : base(connection) { }
    }

}
