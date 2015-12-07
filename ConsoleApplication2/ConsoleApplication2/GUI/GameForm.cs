using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ConsoleApplication2
{
    public partial class GameForm : Form
    {
        System.Timers.Timer timer;
        //Bitmap bm = new Bitmap(1000, 1000);
        private ImageHandler imageHandler;

        // Double Buffering
        BufferedGraphicsContext myContext;
        BufferedGraphics myBuffer;

        // Stuff to draw
        Logic.Player player;
        Logic.AI ai;
        private int numberOfCards; 

        public GameForm()
        {
            InitializeComponent();
            //this.DoubleBuffered = true;

            timer = new System.Timers.Timer(20);
            timer.Elapsed += tick;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

            imageHandler = new ImageHandler();

            // Double Buffering
            myContext = BufferedGraphicsManager.Current;
            myBuffer = myContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
        }

        private void DrawIt()
        {
            Logic.Table.getDrawResources(out player, out ai);

            numberOfCards = player.Hand.numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    DrawCard(player.Hand.viewCard(i), 0 +(80 * i), 0, 0.4f);
                }
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

        private void updateGraphics()
        {
            myBuffer.Graphics.Clear(Color.Black);
            DrawIt();
            myBuffer.Render();
            myBuffer.Render(this.CreateGraphics());
        }

        private void tick(object Object, ElapsedEventArgs e)
        {
            Logic.Table.tick();
            updateGraphics();
        }

    }
}
