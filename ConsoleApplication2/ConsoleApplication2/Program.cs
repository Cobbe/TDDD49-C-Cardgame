using ConsoleApplication2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /* Creates the game instance and sends it to a thread before starting the graphics :) */
            Logic.Table table = new Logic.Table();
            Thread gameThread = new Thread(table.runGame);
            gameThread.Start();

            /* Launches the graphics */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


        }
    }
}