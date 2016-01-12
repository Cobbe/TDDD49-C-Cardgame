using System.Windows.Forms;
using GUI;
using System.Data.Linq;
using System.IO;
using GwentStandalone;

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
                                   AttachDbFileName=" + Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\LINQ\\northwind.mdf");

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