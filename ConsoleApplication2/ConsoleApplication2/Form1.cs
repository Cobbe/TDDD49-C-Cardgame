using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication2
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private int x;
        private bool turn;
        private Image borderImage, cardImage;


        public Form1()
        {
            InitializeComponent();

            borderImage = Image.FromFile(Environment.CurrentDirectory + "\\basic_card_blood.png");
            

            timer = new Timer(new System.ComponentModel.Container());

            x = 0;
            turn = false;
            timer.Enabled = true;
            timer.Interval = 13;
            timer.Tick += new System.EventHandler(this.timer_tic);

            DrawIt();
        }

        private void DrawIt()
        {
            System.Drawing.Graphics graphics = this.CreateGraphics();

            Logic.MonsterCard monster = new Logic.MonsterCard("Dragon", "spits fire", "Dragon.jpg", 5);

            DrawCard(graphics, monster, 0, 0, 1.4f);
            DrawCard(graphics, monster, 300, 150, 0.4f);

        }

        private void DrawCard(System.Drawing.Graphics graphics, Logic.Card card, float x, float y, float scale)
        {
            Console.WriteLine(Environment.CurrentDirectory + "\\" + card.Image);
            cardImage = Image.FromFile(Environment.CurrentDirectory + "\\" + card.Image);

            graphics.DrawImage(borderImage, x, y, 200*scale, 320*scale);
            graphics.DrawImage(cardImage, x+25*scale, y+25*scale, 150*scale, 170*scale);

            graphics.DrawString("Name: " +card.Name, new Font(FontFamily.GenericMonospace, 12*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x+25*scale, y+220*scale);
            graphics.DrawString("Description: " + card.Description, new Font(FontFamily.GenericMonospace, 7*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + 25*scale, y + 235*scale);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawIt();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawIt();
        }

        private void timer_tic(Object sender, EventArgs e)
        {
            if(turn)
            {
                x--;
                if (x == 0)
                    turn = false;
            } else
            {
                x++;
                if (x == 50)
                {
                    turn = true;
                }
            }


            System.Drawing.Graphics graphics = this.CreateGraphics();
            Logic.MonsterCard monster = new Logic.MonsterCard("Dragon", "spits fire", "Dragon.jpg", 5);
            DrawCard(graphics, monster, x, 0, 1.4f);
            //dosuff
        }
    }
}
