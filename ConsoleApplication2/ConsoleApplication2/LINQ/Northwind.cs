using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using Logic;

namespace LINQ
{
    public partial class Northwind : DataContext
    {
        private Table<CardHandler> cardHandlers;
        public Northwind(string connection) : base(connection) { }

        //public Table<CardHandler> getCardHandlers() {
        //    return cardHandlers;
        //}

        
    }

}
