using System.Windows.Forms;
using GUI;
using System.Data.Linq;
using System.IO;
using GwentStandalone;
using GwentStandalone.LINQ;
using Logic;

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
            /* Launches the graphics */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MenuForm menuForm = MenuForm.getMenuForm();
            menuForm.Location = new System.Drawing.Point(50, 50);
            menuForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            menuForm.MaximizeBox = false;

            GameForm gameForm = GameForm.getInstance();
            gameForm.Location = new System.Drawing.Point(50, 50);
            gameForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            gameForm.MaximizeBox = false;

            GameEngine gameEngine = GameEngine.getInstance();

            Application.Run(menuForm);

        }
    }
}