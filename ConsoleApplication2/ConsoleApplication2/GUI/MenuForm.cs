using GwentStandalone;
using Logic;
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
        
        private void resume_button_Click(object sender, EventArgs e)
        {
            LogicEngine.getInstance();
            GameEngine.resumeGame();
            menuForm.Hide();
            GameForm.getInstance().Show();
        }

        private void player_vs_ai_button_Click(object sender, EventArgs e)
        {
            // 1 is to specify a game versus AI
            GameEngine.startNewGame(1);
            menuForm.Hide();
            GameForm.getInstance().Show();
        }

        private void player_vs_player_button_Click(object sender, EventArgs e)
        {
            // 2 is to specify a multiplayer game
            GameEngine.startNewGame(0);
            menuForm.Hide();
            GameForm.getInstance().Show();
            
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
