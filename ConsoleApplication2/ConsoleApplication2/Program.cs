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
            
            Application.Run(menuForm);
        }

        static void mainLoop()
        {
            //main loop


            while (true)
            {
                //get state from database
                Gamestate state = Gamestate.Start;

                switch (state)
                {
                    case Gamestate.Start:
                        Console.WriteLine("start");
                        break;
                    case Gamestate.P1Turn:
                        Console.WriteLine("p1turn");
                        break;
                    case Gamestate.P2Turn:
                        Console.WriteLine("p2Turn");
                        break;
                    case Gamestate.EndGame:
                        Console.WriteLine("End game");
                        break;
                    default:
                        Console.WriteLine("");
                        break;

                }
            }

        }

        static void reset_database(Table table)
        {
            table.cleanDatabase();
            table.generateDatabase();
        }

    }

    enum Gamestate
    {
        Start = 1,
        P1Turn = 2,
        P2Turn = 3,
        EndGame = 4
    }
}