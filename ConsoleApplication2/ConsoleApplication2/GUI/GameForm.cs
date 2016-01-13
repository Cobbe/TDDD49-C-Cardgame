using GwentStandalone.LINQ;
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
        private Object ClickboxLock = new Object();
        private ImageHandler imageHandler;

        public static int numberOfCards;
        public static Card lastClickedCard;
        public static bool activeClick, activePass;

        List<CardClickbox> clickBoxes = new List<CardClickbox>();

        // Double Buffering
        BufferedGraphicsContext myContext;
        BufferedGraphics myBuffer;

        // Stuff to draw
        private int lastClickX = 0, lastClickY = 0;

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
            
            myBuffer.Graphics.DrawImage(imageHandler.getImage("table.png"), 0, 0, Width, Height);

            //Draws all cards and some text
            drawCards();

            //Draw turn info
            String drawstring = "";
            if (LogicEngine.getInstance().state == GameState.P1Turn)
            {
                if (Storage.getPlayer2().pass)
                    drawstring = Storage.getPlayer2().name +" passed, " + Storage.getPlayer1().name + "´s turn";
                else
                    drawstring = Storage.getPlayer1().name+ "'s turn!";
            } else if (LogicEngine.getInstance().state == GameState.P2Turn)
            {
                if (Storage.getPlayer1().pass)
                    drawstring = Storage.getPlayer1().name +" passed, "+ Storage.getPlayer2().name+"´s turn";
                else
                    drawstring = Storage.getPlayer2().name+"'s turn!";
            }
            myBuffer.Graphics.DrawString(drawstring, new Font("Arial", 11 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 30, 250);

            if (round > 3)
            {
                if (wonBattlesPlayer1 > wonBattlesPlayer2)
                {
                    scale = 3;
                    myBuffer.Graphics.DrawString(Storage.getPlayer1().name + " won!", new Font("Arial", 15 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 50, 230);
                }
                else
                {
                    myBuffer.Graphics.DrawString(Storage.getPlayer2().name + " won!", new Font("Arial", 15 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 50, 250);
                }
            }
        }

        private GUI.CardClickbox DrawCard(Logic.Card card, float x, float y, float scale)
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
            
            return clickBox;
        }

        private void drawCards()
        {
            Player currentPlayer = Storage.getPlayer1();
            Player notCurrentPlayer = Storage.getPlayer2();
            int wonBattlesCurrentPlayer = LogicEngine.getWonBattlesPlayer1();
            int wonBattlesNotCurrentPlayer = LogicEngine.getWonBattlesPlayer2();
            if (LogicEngine.getState() == GameState.P2Turn && Storage.getPlayer2().ai == false)
            {
                currentPlayer = Storage.getPlayer2();
                wonBattlesCurrentPlayer = LogicEngine.getWonBattlesPlayer2();
                notCurrentPlayer = Storage.getPlayer1();
                wonBattlesNotCurrentPlayer = LogicEngine.getWonBattlesPlayer1();
            }

            int scale = 2;

            myBuffer.Graphics.DrawString(currentPlayer.name +"(" + wonBattlesCurrentPlayer + ") - Strength: " + currentPlayer.strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 500, 250);
            myBuffer.Graphics.DrawString(notCurrentPlayer.name+"(" + wonBattlesNotCurrentPlayer + ") - Strength: " + notCurrentPlayer.strength, new Font(FontFamily.GenericMonospace, 12 * scale, FontStyle.Bold), new SolidBrush(Color.Blue), 600, 50);
            
            lock (ClickboxLock)
            {
                clickBoxes.Clear();
                // Draw the player's hand
                numberOfCards = currentPlayer.getHand().numberOFCards();
                for (int i = 0; i < numberOfCards; i++)
                {
                    clickBoxes.Add(DrawCard(currentPlayer.getHand().viewCard(i), 20 + (115 * i), 450, 0.5f));
                }
            }

            // Draw the battlefield
            numberOfCards = currentPlayer.getPlayedCards().numberOFCards();
            for (int i = 0; i < numberOfCards; i++)
            {
                DrawCard(currentPlayer.getPlayedCards().viewCard(i), 20 + (90 * i), 300, 0.4f);
            }

            numberOfCards = notCurrentPlayer.getPlayedCards().numberOFCards();
            for (int i = 0; i < numberOfCards; i++)
            {
                DrawCard(notCurrentPlayer.getPlayedCards().viewCard(i), 20 + (90 * i), 100, 0.4f);
            }

            // Draw the AI's hand (just the cardback)
            numberOfCards = notCurrentPlayer.getHand().numberOFCards();
            for (int i = 0; i < numberOfCards; i++)
            {
                DrawCardBackside(20 + (45 * i), 20, 0.2f);
            }
        }

        private void DrawCardBackside(float x, float y, float scale)
        {
            myBuffer.Graphics.DrawImage(imageHandler.getImage(ImageHandler.CARDBACK), x, y, 200 * scale, 320 * scale);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.Equals((sender as Button).Name, @"CloseButton"))
            {
                // Do something proper to CloseButton.
            }
            else
            {
                // Then assume that X has been clicked and act accordingly.
            }
            lock (gameFormLock)
            {
                GwentStandalone.GameEngine.getInstance().closeGame();
                this.Close();
                MenuForm.getMenuForm().Close();
                Environment.Exit(1);
                Application.Exit();
            }
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

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastClickX = e.X;
                lastClickY = e.Y;
                Console.WriteLine("Clicked! x:" +lastClickX +" y:"+lastClickY);

                lock (ClickboxLock)
                {
                    Console.WriteLine(clickBoxes.Count + "Clickboxes");
                    for (int i = 0; i < clickBoxes.Count; i++)
                    {
                        if (clickBoxes[i].inBox(lastClickX, lastClickY))
                        {
                            lastClickedCard = clickBoxes[i].card;

                            activeClick = true;
                        }
                    }
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                activePass = true;
            }
        }

    }
}
