using ConsoleApplication2.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace ConsoleApplication2
{
    public partial class GameForm : Form
    {
        private static GameForm gameForm;
        private Object gameFormLock = new Object();
        //private System.Timers.Timer timer;
        //Bitmap bm = new Bitmap(1000, 1000);
        private ImageHandler imageHandler;

        List<CardClickbox> clickBoxes = new List<CardClickbox>();

        // Double Buffering
        BufferedGraphicsContext myContext;
        BufferedGraphics myBuffer;

        // Stuff to draw
        Logic.Player player;
        Logic.AI ai;
        private int numberOfCards, playedBattles, wonBattles, lastClickedBox;
        private bool win, activeClick;

        private int lastClickX = 0, lastClickY = 0;

        public bool ActiveClick
        {
            get
            {
                return activeClick;
            }

            set
            {
                activeClick = value;
            }
        }

        public int LastClickedBox
        {
            get
            {
                return lastClickedBox;
            }

            set
            {
                lastClickedBox = value;
            }
        }

        /*
        public System.Timers.Timer Timer
        {
            get
            {
                return timer;
            }

            set
            {
                timer = value;
            }
        }
        */

        public static GameForm getGameForm()
        {
            if(gameForm == null)
            {
                return gameForm = new GameForm();
            } else
            {
                return gameForm;
            }
        }

        private GameForm()
        {
            InitializeComponent();


            //Mouse
            this.MouseClick += mouseClick;

            //Timer
            /*
            Timer = new System.Timers.Timer(1000);
            Timer.Elapsed += tick;
            Timer.AutoReset = true;
            Timer.Enabled = false;
            */

            imageHandler = new ImageHandler();

            // Double Buffering
            myContext = BufferedGraphicsManager.Current;
            myBuffer = myContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
        }

        private void DrawIt()
        {
            Logic.Table.getDrawResources(out player, out ai, out playedBattles, out wonBattles, out win);
            int scale = 2;

            if (playedBattles > 3)
            {
                if (win)
                {
                    scale = 3;
                    myBuffer.Graphics.DrawString("Victory!", new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 50, 230);
                } else
                {
                    myBuffer.Graphics.DrawString("Defeat!", new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 50, 250);
                }
            }
            else
            {
                clickBoxes.Clear();
                myBuffer.Graphics.DrawImage(imageHandler.getImage("table.png"), 0, 0, Width, Height);

                myBuffer.Graphics.DrawString("Player(" +wonBattles +") - Strength: " + player.Strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 500, 250);
                myBuffer.Graphics.DrawString("AI(" +(playedBattles-wonBattles) +") - Strength: " + ai.Strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 600, 50);

                // Draw the player's hand
                numberOfCards = player.Hand.numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    clickBoxes.Add(DrawCard(player.Hand.viewCard(i), 20 + (115 * i), 450, 0.5f));
                }

                // Draw the battlefield
                numberOfCards = player.PlayedCards.numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    DrawCard(player.PlayedCards.viewCard(i), 20 + (90 * i), 300, 0.4f);
                }

                numberOfCards = ai.PlayedCards.numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    DrawCard(ai.PlayedCards.viewCard(i), 20 + (90 * i), 100, 0.4f);
                }

                // Draw the AI's hand (just the cardback)
                numberOfCards = ai.Hand.numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    DrawCard(20 + (45 * i), 20, 0.2f);
                }
            }
        }

        private GUI.CardClickbox DrawCard(Logic.Card card, float x, float y, float scale)
        {
            Image cardImage = imageHandler.getImage(card.Image);
            myBuffer.Graphics.DrawImage(imageHandler.getImage(ImageHandler.CARD_BORDER), x, y, 200*scale, 320*scale);
            myBuffer.Graphics.DrawImage(cardImage, x+25*scale, y+25*scale, 150*scale, 170*scale);

            GUI.CardClickbox clickBox = new GUI.CardClickbox(x, y, 200 * scale, 320 * scale, card);

            myBuffer.Graphics.DrawString("Name: " +card.Name, new Font(FontFamily.GenericMonospace, 12*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x+25*scale, y+220*scale);
            myBuffer.Graphics.DrawString("Description: " + card.Description, new Font(FontFamily.GenericMonospace, 7*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + 25*scale, y + 235*scale);
            if(card is Logic.MonsterCard)
            {
                Logic.MonsterCard temp = (Logic.MonsterCard)card;
                myBuffer.Graphics.DrawString("Strength: " + temp.Strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + 25 * scale, y + 250 * scale);
            }

            return clickBox;
        }

        private void DrawCard(float x, float y, float scale)
        {
            myBuffer.Graphics.DrawImage(imageHandler.getImage(ImageHandler.CARDBACK), x, y, 200 * scale, 320 * scale);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void updateGraphics()
        {
            lock (gameFormLock)
            {
                DrawIt();
                myBuffer.Render();
                myBuffer.Render(CreateGraphics());
            }
        }

        /*
        private void tick(object Object, ElapsedEventArgs e)
        {
            Logic.Table.tick();
            updateGraphics();
        }
        */

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastClickX = e.X;
                lastClickY = e.Y;
                // Insert code which identifies which clickBox has been clicked on.
                for(int i = 0; i<clickBoxes.Count; i++)
                {
                    if(clickBoxes[i].inBox(lastClickX, lastClickY))
                    {
                        LastClickedBox = i;
                        ActiveClick = true;
                        Console.WriteLine("Clicked box #"+i);
                    }
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                Logic.Table.getTableInstance().PlayerPass = true;
            }
        }

    }
}
