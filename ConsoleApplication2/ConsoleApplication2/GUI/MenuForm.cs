using GwentStandalone;
using System;
using System.Windows.Forms;

namespace GUI
{
    public partial class MenuForm : Form
    {
        private static MenuForm menuForm;

        public static MenuForm getMenuForm()
        {
            if(menuForm == null)
            {
                return menuForm = new MenuForm();
            } else
            {
                return menuForm;
            }
        }

        private MenuForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameEngine.resumeGame();
            menuForm.Hide();
            GameForm.getInstance().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 1 is to specify a game versus AI
            GameEngine.startNewGame(1);
            menuForm.Hide();
            GameForm.getInstance().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 2 is to specify a multiplayer game
            GameEngine.startNewGame(0);
            menuForm.Hide();
            GameForm.getInstance().Show();
            
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }
    }
}
