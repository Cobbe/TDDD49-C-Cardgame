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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            // Setup LINQ
            // DataContext takes a connection string 
            DataContext db = new DataContext(@"Data Source=(localdb)\mssqllocaldb;
                                   Integrated Security=true;
                                   AttachDbFileName="+ Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\LINQ\\northwind.mdf");
            
            /* Launches the graphics */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /* Creates the game instance and the game timer*/
            Table table = Table.getTableInstance();
            
            MenuForm menuForm = MenuForm.getMenuForm();
            menuForm.Location = new System.Drawing.Point(50, 50);

            GameForm gameForm = GameForm.getGameForm();
            gameForm.Location = new System.Drawing.Point(50, 50);
            
            //if new game
            table.cleanDatabase(db);
            table.generateDatabase(db);

            //testprints deck
            Table <Player> players = table.getPlayers(db);
            foreach (Player player in players)
            {
                Console.WriteLine("player: " + player.name);
                Console.WriteLine("  Deck");
                foreach (Card card in player.getDeck(db).getCards(db))
                {
                    Console.WriteLine("    "+card.name +": " +card.description +" ("+card.strength +")");
                    //player.getHand(db).moveCardHere(card, db);
                }
            }
            Application.Run(menuForm);
        }
        
    }
}