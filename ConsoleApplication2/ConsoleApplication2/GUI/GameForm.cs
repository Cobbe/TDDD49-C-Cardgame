using System;
using System.Drawing;
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

        private int lastClickX = 0, lastClickY = 0;

        public GameForm()
        {
            InitializeComponent();

            //Mouse
            this.MouseClick += mouseClick;

            //Timer
            timer = new System.Timers.Timer(4000);
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
                DrawCard(player.Hand.viewCard(i), lastClickX +(80 * i), lastClickY, 0.4f);
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

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Console.WriteLine("Mouse clicked x:" +e.X +" y: " +e.Y);
                lastClickX = e.X;
                lastClickY = e.Y;
            }
        }

    }
}
