using GUI;
using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GwentStandalone
{
    class GameEngine
    {
        private static GameEngine gameEngine;
        public Timer timer;
        private BackgroundWorker gameWorker, graphicsWorker;

        private GameEngine()
        {
            gameWorker = new BackgroundWorker();
            gameWorker.DoWork += gameLoop;
            timer = new Timer(200);
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = false;
        }

        public static GameEngine getInstance()
        {
            if(gameEngine == null)
            {
                return gameEngine = new GameEngine();
            } else
            {
                return gameEngine;
            }
        }

        private void gameLoop(object Object, DoWorkEventArgs e)
        {
            //Update GUI
            //Console.WriteLine("Updating Graphics");
            GameForm.getInstance().updateGraphics();
            bool waitingForInput;

            // Alpha-version is currently using two AIs (this is not indicated in the DB)
            switch (LogicEngine.getInstance().state)
            {
                case GameState.Start:
                    //Console.WriteLine("start");
                    LogicEngine.nextRound();
                    LogicEngine.getPlayer1().drawCards(10);
                    LogicEngine.getPlayer2().drawCards(10);
                    LogicEngine.getInstance().updateGamestate(GameState.P1Turn);
                    break;
                case GameState.P1Turn:
                    //Console.WriteLine("p1turn");
                    waitingForInput = LogicEngine.getPlayer1().determineAndPerformAction();
                    if (!waitingForInput)
                    {
                        LogicEngine.getInstance().updateGamestate(GameState.P2Turn);
                    }  
                    break;
                case GameState.P2Turn:
                    //Console.WriteLine("p2Turn");
                    waitingForInput = LogicEngine.getPlayer2().determineAndPerformAction();
                    if (!waitingForInput)
                    {
                        LogicEngine.getInstance().updateGamestate(GameState.EndTurn);
                    }
                    break;
                case GameState.EndTurn:
                    //Console.WriteLine("End Round");
                    LogicEngine.getInstance().updateGamestate(LogicEngine.determineRound());
                    break;
                case GameState.EndGame:
                    Console.WriteLine("End game");
                    break;
                default:
                    //Console.WriteLine("Game is not running!");
                    break;
            }
        }

        private void reset_database()
        {
            LogicEngine.cleanDatabase();
            LogicEngine.generateDatabase();
        }

        public static void resumeGame()
        {
            //get state from database
            gameEngine.timer.Start();
            Console.WriteLine("Resumed Game");
        }

        public static void startNewGame(int gameMode)
        {
            if (gameMode == 1)
            {
                gameEngine.reset_database();
                LogicEngine.getInstance().updateGamestate(GameState.Start);
                gameEngine.timer.Start();
                Console.WriteLine("Started Game");
            }
            else if (gameMode == 2)
            {
                gameEngine.reset_database();
                LogicEngine.getInstance().updateGamestate(GameState.Start);
                gameEngine.timer.Start();
                Console.WriteLine("Started Game");
            }

        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!gameWorker.IsBusy)
            {
                gameWorker.RunWorkerAsync();
            }
        }

    }
    
}

