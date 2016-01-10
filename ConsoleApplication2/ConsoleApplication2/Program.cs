using System;
using System.Windows.Forms;
using GUI;
using System.Data.Linq;
using Logic;
using System.IO;

namespace GwentStandAlone
{
    static class Program
    {
        public static DataContext db;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            // Setup LINQ
            // DataContext takes a connection string 
            db = new DataContext(@"Data Source=(localdb)\mssqllocaldb;
                                   Integrated Security=true;
                                   AttachDbFileName="+ Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\LINQ\\northwind.mdf");

            /* Launches the graphics */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /* Creates the game instance and the game timer*/
            Table table = Table.getTableInstance();

            //if new game
            table.cleanDatabase();
            table.generateDatabase();

            MenuForm menuForm = MenuForm.getMenuForm();
            menuForm.Location = new System.Drawing.Point(50, 50);

            GameForm gameForm = GameForm.getGameForm();
            gameForm.Location = new System.Drawing.Point(50, 50);

            //testprints deck
            Table <Player> players = table.getPlayers();
            foreach (Player player in players)
            {
                Console.WriteLine("player: " + player.name);
                Console.WriteLine("  Deck");
                foreach (Card card in player.getDeck().getCards())
                {
                    Console.WriteLine("    "+card.name +": " +card.description +" ("+card.strength +")");
                    //player.getHand(db).moveCardHere(card, db);
                }
            }
            Application.Run(menuForm);
        }
        
    }
}