using GUI;
using GwentStandalone.LINQ;
using GwentStandalone.Logic;
using GwentStandAlone;
using Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
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
        private BackgroundWorker gameWorker;

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
            Storage.updateDB();

            //Update GUI
            //Console.WriteLine("Updating Graphics");
            GameForm.getInstance().updateGraphics();
            bool waitingForInput;

            switch (LogicEngine.getInstance().state)
            {
                case GameState.Start:
                    LogicEngine.nextRound();
                    Storage.getPlayer1().drawCards(10);
                    Storage.getPlayer2().drawCards(10);
                    LogicEngine.getInstance().updateGamestate(GameState.P1Turn);
                    break;
                case GameState.P1Turn:
                    waitingForInput = Storage.getPlayer1().determineAndPerformAction();
                    if (!waitingForInput)
                    {
                        LogicEngine.getInstance().updateGamestate(GameState.P2Turn);
                    }  
                    break;
                case GameState.P2Turn:
                    waitingForInput = Storage.getPlayer2().determineAndPerformAction();
                    if (!waitingForInput)
                    {
                        LogicEngine.getInstance().updateGamestate(GameState.EndTurn);
                    }
                    break;
                case GameState.EndTurn:
                    LogicEngine.getInstance().updateGamestate(RuleEngine.determineRound());
                    break;
                case GameState.EndGame:
                    break;
                default:
                    Console.WriteLine("Game is not running!");
                    break;
            }
        }

        public void closeGame()
        {
            timer.Stop();
            gameWorker.CancelAsync();
        }

        private void reset_database(int gameMode)
        {
            Storage.cleanDatabase();
            Storage.generateDatabase(gameMode, 20);
        }

        public static void resumeGame()
        {
            gameEngine.timer.Start();
            Console.WriteLine("Resumed Game");
        }

        public static void startNewGame(int gameMode)
        {
                gameEngine.reset_database(gameMode);
                LogicEngine.getInstance().updateGamestate(GameState.Start);
                gameEngine.timer.Start();
                Console.WriteLine("Started Game");
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

