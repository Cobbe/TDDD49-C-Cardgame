using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using GUI;

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
            // DataContext takes a connection string 
            DataContext db = new DataContext("c:\\northwind\\northwnd.mdf");
            // Get a typed table to run queries
            Table<CardHandlers> cardHandlers = db.GetTable<CardHandlers>();

            /* Launches the graphics */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /* Creates the game instance and the game timer*/
            Logic.Table table = Logic.Table.getTableInstance();

            MenuForm menuForm = MenuForm.getMenuForm();
            menuForm.Location = new System.Drawing.Point(50, 50);

            GameForm gameForm = GameForm.getGameForm();
            gameForm.Location = new System.Drawing.Point(50, 50);

            Application.Run(menuForm);

        }

    }
}