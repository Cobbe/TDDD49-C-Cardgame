using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using GUI;
using System.Data.Linq;
using Logic;
using System.Data.Linq.Mapping;
using System.Xml.Linq;

namespace GwentStandAlone
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            // Setup LINQ
            // DataContext takes a connection string 
            //DataContext db = new DataContext("D:\\Cobbe\\Git Projekt\\tddd49-csharp-projekt\\ConsoleApplication2\\ConsoleApplication2\\LINQ\\northwind.mdf");
            DataContext db = new DataContext(@"Data Source=(localdb)\mssqllocaldb;
                                   Integrated Security=true;
                                   AttachDbFileName=D:\Cobbe\Git Projekt\tddd49-csharp-projekt\ConsoleApplication2\ConsoleApplication2\LINQ\northwind.mdf");
            // Get a typed table to run queries
            Table<Customer> Customers = db.GetTable<Customer>();
            // Query for customers from London
            var q =
               from c in Customers
               where c.City == "London"
               select c;
            foreach (var cust in q)
                Console.WriteLine("id = {0}, City = {1}", cust.CustomerID, cust.City);

            /* Launches the graphics */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /* Creates the game instance and the game timer*/
            Table table = Table.getTableInstance();

            

            MenuForm menuForm = MenuForm.getMenuForm();
            menuForm.Location = new System.Drawing.Point(50, 50);

            GameForm gameForm = GameForm.getGameForm();
            gameForm.Location = new System.Drawing.Point(50, 50);

            Application.Run(menuForm);

        }

        [Table(Name = "Customer")]
        public class Customer
        {
            [Column(IsPrimaryKey = true)]
            public int CustomerID;
            [Column]
            public String City;
        }
    }
}