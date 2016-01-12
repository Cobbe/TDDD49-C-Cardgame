using GwentStandAlone;
using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace GUI
{
    public partial class GameForm : Form
    {
        private static GameForm gameForm;
        private Object gameFormLock = new Object();
        private ImageHandler imageHandler;

        List<CardClickbox> clickBoxes = new List<CardClickbox>();

        // Double Buffering
        BufferedGraphicsContext myContext;
        BufferedGraphics myBuffer;

        // Stuff to draw
        private int numberOfCards, lastClickedBox;
        private bool activeClick;

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

        public static GameForm getInstance()
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

            imageHandler = new ImageHandler();

            // Double Buffering
            myContext = BufferedGraphicsManager.Current;
            myBuffer = myContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
        }

        private void DrawIt()
        {
            int scale = 2;
            int round = LogicEngine.getRound();
            int wonBattlesPlayer1 = LogicEngine.getWonBattlesPlayer1();
            int wonBattlesPlayer2 = LogicEngine.getWonBattlesPlayer2();

            if (round > 3)
            {
                if (wonBattlesPlayer1 > wonBattlesPlayer2)
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

                myBuffer.Graphics.DrawString("Player(" + wonBattlesPlayer1 + ") - Strength: " + LogicEngine.getPlayer1().strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 500, 250);
                myBuffer.Graphics.DrawString("AI(" + wonBattlesPlayer2 + ") - Strength: " + LogicEngine.getPlayer2().strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 600, 50);

                // Draw the player's hand
                numberOfCards = LogicEngine.getPlayer1().getHand().numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    clickBoxes.Add(DrawCard2(LogicEngine.getPlayer1().getHand().viewCard(i), 20 + (115 * i), 450, 0.5f));
                }

                // Draw the battlefield
                numberOfCards = LogicEngine.getPlayer1().getPlayedCards().numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    DrawCard2(LogicEngine.getPlayer1().getPlayedCards().viewCard(i), 20 + (90 * i), 300, 0.4f);
                }

                numberOfCards = LogicEngine.getPlayer2().getPlayedCards().numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    DrawCard2(LogicEngine.getPlayer2().getPlayedCards().viewCard(i), 20 + (90 * i), 100, 0.4f);
                }

                // Draw the AI's hand (just the cardback)
                numberOfCards = LogicEngine.getPlayer2().getHand().numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    DrawCard(20 + (45 * i), 20, 0.2f);
                }
            }
        }

        private GUI.CardClickbox DrawCard(Logic.Card card, float x, float y, float scale)
        {
            Image cardImage = imageHandler.getImage(card.image);
            myBuffer.Graphics.DrawImage(imageHandler.getImage(ImageHandler.CARD_BORDER), x, y, 200*scale, 320*scale);
            myBuffer.Graphics.DrawImage(cardImage, x+25*scale, y+25*scale, 150*scale, 170*scale);

            GUI.CardClickbox clickBox = new GUI.CardClickbox(x, y, 200 * scale, 320 * scale, card);

            myBuffer.Graphics.DrawString("Name: " +card.name, new Font(FontFamily.GenericMonospace, 12*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x+25*scale, y+220*scale);
            myBuffer.Graphics.DrawString("Description: " + card.description, new Font(FontFamily.GenericMonospace, 7*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + 25*scale, y + 235*scale);
            myBuffer.Graphics.DrawString("Strength: " + card.strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + 25 * scale, y + 250 * scale);

            return clickBox;
        }

        private GUI.CardClickbox DrawCard2(Logic.Card card, float x, float y, float scale)
        {
            Image cardImage = imageHandler.getImage(card.image);
            myBuffer.Graphics.DrawImage(imageHandler.getImage(ImageHandler.CARD_BORDER), x, y, 200 * scale, 320 * scale);
            myBuffer.Graphics.DrawImage(cardImage, x + 25 * scale, y + 25 * scale, 150 * scale, 170 * scale);

            GUI.CardClickbox clickBox = new GUI.CardClickbox(x, y, 200 * scale, 320 * scale, card);

            myBuffer.Graphics.FillEllipse(new SolidBrush(Color.BurlyWood), x, y, 55 * scale, 55 * scale);
            myBuffer.Graphics.DrawEllipse(new Pen(Color.Black), x, y, 55 * scale, 55 * scale);

            float fontSize = (card.strength < 10) ? 37 : 30;
            float xDiff = (card.strength < 10) ? 6 : -1;
            float yDiff = (card.strength < 10) ? 2 : 9;
            myBuffer.Graphics.DrawString(card.strength.ToString(), new Font(FontFamily.GenericMonospace, fontSize * scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + xDiff * scale, y + yDiff * scale);

            myBuffer.Graphics.DrawString(card.name, new Font("Arial", 17*scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + 25 * scale, y + 220 * scale);
            myBuffer.Graphics.DrawString(card.description, new Font("Arial", 15 * scale, FontStyle.Regular), new SolidBrush(Color.Red), x + 25 * scale, y + 250 * scale);
            //myBuffer.Graphics.DrawString("Strength: " + card.strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), x + 25 * scale, y + 270 * scale);
            
            return clickBox;
        }

        private void DrawCard(float x, float y, float scale)
        {
            myBuffer.Graphics.DrawImage(imageHandler.getImage(ImageHandler.CARDBACK), x, y, 200 * scale, 320 * scale);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        //public void updateGraphics(object Object, DoWorkEventArgs e)
        public void updateGraphics()
        {
            lock (gameFormLock)
            {
                DrawIt();
                myBuffer.Render();
                myBuffer.Render(CreateGraphics());
            }
        }

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
                // Make a buffer variable instead of going straight for the DB
                //Logic.LogicEngine.getInstance().getPlayer1().setPass(true);
            }
        }

    }
}
