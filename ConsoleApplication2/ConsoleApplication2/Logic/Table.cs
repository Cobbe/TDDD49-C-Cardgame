﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ConsoleApplication2;

namespace Logic
{
    class Table
    {
        private static Table table;
        private Player player;
        private AI ai;
        
        private int cardToPlay;
        private MonsterCard playMonster;
        private SpecialCard playSpecial;
        private bool win = false;
        private Timer timer;
        private BackgroundWorker worker;
        private int waitBetweenActions = 500; // In milliseconds
        private bool playerPass, aiPass, firstTurn;
        private int playedBattles, wonBattles;

        private static Random tableRng = new Random();

        protected Player Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        protected AI Ai
        {
            get
            {
                return ai;
            }

            set
            {
                ai = value;
            }
        }

        protected int CardToPlay
        {
            get
            {
                return cardToPlay;
            }

            set
            {
                cardToPlay = value;
            }
        }

        internal SpecialCard PlaySpecial
        {
            get
            {
                return playSpecial;
            }

            set
            {
                playSpecial = value;
            }
        }

        internal MonsterCard PlayMonster
        {
            get
            {
                return playMonster;
            }

            set
            {
                playMonster = value;
            }
        }

        protected bool Win
        {
            get
            {
                return win;
            }

            set
            {
                win = value;
            }
        }

        public Timer Timer
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

        public int PlayedBattles
        {
            get
            {
                return playedBattles;
            }

            set
            {
                playedBattles = value;
            }
        }

        public int WonBattles
        {
            get
            {
                return wonBattles;
            }

            set
            {
                wonBattles = value;
            }
        }

        public bool PlayerPass
        {
            get
            {
                return playerPass;
            }

            set
            {
                playerPass = value;
            }
        }

        public bool AiPass
        {
            get
            {
                return aiPass;
            }

            set
            {
                aiPass = value;
            }
        }

        public static Table getTableInstance()
        {
            if(table != null)
            {
                return table;
            } else
            {
                return table = new Table();
            }
        }

        private Table()
        {
            initializeGame();

            worker = new BackgroundWorker();
            worker.DoWork += runGame;
            Timer = new Timer(100);
            Timer.Elapsed += timer_Elapsed;
            Timer.AutoReset = true;
            Timer.Enabled = false;
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
            //ConsoleApplication2.GameForm.getGameForm().updateGraphics();
        }

        public void runGame(object Object, DoWorkEventArgs e)
        {
            if (PlayedBattles < 3)
            {
                if ((!PlayerPass || !aiPass) && ((player.Hand.numberOFCards() > 0 || ai.Hand.numberOFCards() > 0) || firstTurn))
                {
                    // Turn-based

                    // Start by drawing cards
                    if (firstTurn)
                    {
                        drawCards(Player, 10);
                        ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        System.Threading.Thread.Sleep(waitBetweenActions);

                        drawCards(Ai, 10);
                        ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        System.Threading.Thread.Sleep(waitBetweenActions);
                        firstTurn = false;

                    }
                    // Play card
                    if(player.Hand.numberOFCards() > 0 && !playerPass)
                    {
                        while (!GameForm.getGameForm().ActiveClick)
                        {
                            if (playerPass)
                            {
                                break;
                            }
                        }
                        if (!playerPass)
                        {
                            playCard(player, player.Hand.getCard(GameForm.getGameForm().LastClickedBox));
                            GameForm.getGameForm().ActiveClick = false;
                            ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                            System.Threading.Thread.Sleep(waitBetweenActions);
                        }
                        
                    }

                    // Play card
                    if (ai.Hand.numberOFCards() > 0 && ai.Strength<(player.Strength+10) && !aiPass)
                    {
                        playCard(ai, ai.Hand.getCard(getStrongestCard(ai)));
                        ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                        System.Threading.Thread.Sleep(waitBetweenActions);
                    } else
                    {
                        aiPass = true;
                    }
                }
                else
                {
                    if (player.Strength > ai.Strength)
                    {
                        wonBattles++;
                    }
                    playedBattles++;
                    aiPass = false;
                    PlayerPass = false;
                    player.PlayedCards.clear();
                    ai.PlayedCards.clear();
                    player.Strength = 0;
                    ai.Strength = 0;
                    ConsoleApplication2.GameForm.getGameForm().updateGraphics();
                }
            } else
            {
                if (WonBattles >=2 )
                {
                    win = true;
                }
                else
                {
                    win = false;
                }
                PlayedBattles++;
                ConsoleApplication2.GameForm.getGameForm().updateGraphics();
            }
        }

        protected void initializeGame()
        {
            Player = new Player();
            Ai = new AI();
            initialDecks();
            playedBattles = 0;
            wonBattles = 0;
            PlayerPass = false;
            aiPass = false;
            firstTurn = true;
        }
        
        protected void initialDecks()
        {
            // For player
            player.Deck.clear();
            player.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Wind Drake", "Has sharp claws", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Wind Drake", "Has sharp claws", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Wind Drake", "Has sharp claws", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            player.Deck.addCard(new MonsterCard("Wind Drake", "Has sharp claws", "dragon.png", 8));
            player.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            player.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            player.Deck.addCard(new MonsterCard("Orc Champion", "Waaaaagh!!!", "warrior_orc.png", 12));
            player.Deck.shuffle();

            // For AI
            ai.Deck.clear();
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 5));
            ai.Deck.addCard(new MonsterCard("Wind Dragon", "Summons tornados", "dragon.png", 12));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Fire Dragon", "Breathes fire", "dragon.png", 10));
            ai.Deck.addCard(new MonsterCard("Orc", "Waaagh!!", "warrior_orc.png", 2));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 5));
            ai.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            ai.Deck.addCard(new MonsterCard("Orc Commander", "Waaagh!!!", "warrior_orc.png", 5));
            ai.Deck.addCard(new MonsterCard("Witch", "Dark Sorcery", "witch.png", 3));
            ai.Deck.shuffle();
        }

        protected void playCard(Player player, Card card)
        {
            if (card is MonsterCard)
            {
                PlayMonster = (MonsterCard)card;
                player.Strength += PlayMonster.Strength;
                player.PlayedCards.addCard(card);

            }
            else
            {
                PlaySpecial = (SpecialCard)card;
                player.PlayedCards.addCard(card);
            }

        }

        protected void drawCards(Player player, int number)
        {
            for(int i = 0; i < number; i++)
            {
                player.Hand.addCard(player.Deck.drawCard());
            }
        }

        protected int getStrongestCard(Player player)
        {
            int indexOfHigh = 0;
            /*
            for(int i = 1; i<player.Hand.numberOFCards(); i++)
            {
                if(player.Hand.viewCard(indexOfHigh) is MonsterCard)
                {
                    if(player.Hand.viewCard(i) is MonsterCard)
                    {
                        MonsterCard temp1 = (MonsterCard)player.Hand.viewCard(indexOfHigh);
                        MonsterCard temp2 = (MonsterCard)player.Hand.viewCard(i);
                        if(temp2.Strength > temp1.Strength)
                        {
                            indexOfHigh = i;
                        }
                    }
                }
            }*/
            indexOfHigh = -1;
            int strongest = -1;
            for (int i = 0; i < player.Hand.numberOFCards(); i++)
            {
                if (player.Hand.viewCard(i) is MonsterCard)
                {
                    int testCardStrength = ((MonsterCard)player.Hand.viewCard(i)).Strength;
                    if(testCardStrength > strongest)
                    {
                        strongest = testCardStrength;
                        indexOfHigh = i;
                    }
                }
            }
            return indexOfHigh;
        }

        public static void getDrawResources(out Player playerOUT, out AI aiOUT, out int playedBattlesOUT, out int wonBattlesOUT, out bool winOUT)
        {
            playerOUT = table.player;
            aiOUT = table.ai;
            playedBattlesOUT = table.playedBattles;
            wonBattlesOUT = table.wonBattles;
            winOUT = table.win;
        }
    }
}
