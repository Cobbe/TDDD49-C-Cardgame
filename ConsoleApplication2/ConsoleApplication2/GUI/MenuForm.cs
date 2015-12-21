using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication2.GUI
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
            GameForm.getGameForm().Show();
            Logic.Table.createTableInstance().Timer.Start();
            menuForm.Hide();
        }

    }
}
