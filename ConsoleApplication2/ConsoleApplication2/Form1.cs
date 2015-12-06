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
        //Bitmap bm = new Bitmap(1000, 1000);
        private ImageHandler imageHandler;

        // Double Buffering
        BufferedGraphicsContext myContext;
        BufferedGraphics myBuffer;


        public Form1()
        {
            InitializeComponent();
            //this.DoubleBuffered = true;

            imageHandler = new ImageHandler();

            // Double Buffering
            myContext = BufferedGraphicsManager.Current;
            myBuffer = myContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);

            timer = new Timer(new System.ComponentModel.Container());

            x = 0;
            turn = false;
            timer.Enabled = true;
            timer.Interval = 13;
            timer.Tick += new System.EventHandler(this.timer_tic);

        }

        private void DrawIt()
        {

            Logic.MonsterCard monster = new Logic.MonsterCard("Dragon", "spits fire", "Dragon.jpg", 5);
            DrawCard(monster, x, 0, 1.4f);
            //DrawCard(monster, 300, 150, 0.4f);

            
        }

        private void DrawCard(Logic.Card card, float x, float y, float scale)
        {
            Image cardImage = imageHandler.getImage(card.Image);
            myBuffer.Graphics.DrawImage(imageHandler.getImage(ImageHandler.CARD_BORDER), x, y, 200*scale, 320*scale);
            myBuffer.Graphics.DrawImage(cardImage, x+25*scale, y+25*scale, 150*scale, 170*scale);

            myBuffer.Graphics.DrawString("Name: " +card.Name, new Font(FontFamily.GenericMonospace, 12*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x+25*scale, y+220*scale);
            myBuffer.Graphics.DrawString("Description: " + card.Description, new Font(FontFamily.GenericMonospace, 7*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + 25*scale, y + 235*scale);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
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

            myBuffer.Graphics.Clear(Color.Black);
            DrawIt();
            myBuffer.Render();
            myBuffer.Render(this.CreateGraphics());
        }

    }
}
