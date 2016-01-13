using GwentStandalone;
using GwentStandalone.LINQ;
using GwentStandAlone;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;


namespace Logic
{
    [Table(Name = "LogicEngine")]
    public class LogicEngine
    {
        private static LogicEngine logicEngine;

        [Column(IsPrimaryKey = true)]
        public int id;
        [Column]
        public int round;
        [Column]
        public int wonBattlesPlayer1;
        [Column]
        public int wonBattlesPlayer2;
        [Column]
        public GameState state;

        public LogicEngine() : base()
        {

        }

        public static LogicEngine getInstance()
        {
            if(logicEngine == null)
            {
                return logicEngine = Storage.getLogicEngine();
            }
            return logicEngine;
        }


        public static int getRound()
        {
            return logicEngine.round;
        }

        public static GameState getState()
        {
            return logicEngine.state;
        }

        public static int getWonBattlesPlayer1()
        {
            return logicEngine.wonBattlesPlayer1;
        }

        public static int getWonBattlesPlayer2()
        {
            return logicEngine.wonBattlesPlayer2;
        }

        /*
        public static int getRound()
        {
            LogicEngine temp = getDBInstance();
            if(temp == null)
            {
                return -1;
            }
            return getDBInstance().round;
        }

        public static GameState getState()
        {
            LogicEngine temp = getDBInstance();
            if (temp == null)
            {
                return GameState.Problem;
            }
            return getDBInstance().state;
        }

        public static int getWonBattlesPlayer1()
        {
            LogicEngine temp = getDBInstance();
            if (temp == null)
            {
                return -1;
            }
            return getDBInstance().wonBattlesPlayer1;
        }

        public static int getWonBattlesPlayer2()
        {
            LogicEngine temp = getDBInstance();
            if (temp == null)
            {
                return -1;
            }
            return getDBInstance().wonBattlesPlayer2;
        }
        */

        public static void nextRound()
        {
            Storage.getPlayer1().setPass(false);
            Storage.getPlayer2().setPass(false);
            Storage.getPlayer1().setStrength(0);
            Storage.getPlayer2().setStrength(0);
            Storage.getPlayer1().movePlayedCardsToUsed();
            Storage.getPlayer2().movePlayedCardsToUsed();
            Storage.newRound();
        }

        public static void player1Won()
        {
            Storage.incresePlayerWon(Storage.getPlayer1().id);
        }

        public static void player2Won()
        {
            Storage.incresePlayerWon(Storage.getPlayer2().id);
        }

        public static GameState determineRound()
        {
            
            //Console.WriteLine("P1Pass: " + getPlayer1().pass + " P2Pass: " + getPlayer2().pass);
            if (Storage.getPlayer1().pass == true && Storage.getPlayer2().pass == true)
            {
                if(Storage.getPlayer1().strength > Storage.getPlayer2().strength)
                {
                    player1Won();
                } else
                {
                    player2Won();
                }
                if((getWonBattlesPlayer1() == 2 && getWonBattlesPlayer2() == 0) || (getWonBattlesPlayer1() == 0 && getWonBattlesPlayer2() == 2))
                {
                    // Add an extra round
                    nextRound();
                }
            nextRound();
            }
            if (getRound() > 3)
            {
                Console.WriteLine("It is over");
                nextRound();
                return GameState.EndGame;
            }
            
            return GameState.P1Turn;
        }
        

        /* Implement this method?
        public static void getDrawResources(out int playedBattlesOUT, out int wonBattlesOUT, out bool winOUT)
        {
            
        }
        */
        public void updateGamestate(GameState state)
        {
            Storage.updateGamestate(this.id, state);
        }

    }

    public enum GameState
    {
        Start = 1,
        P1Turn = 2,
        P2Turn = 3,
        EndTurn = 4,
        EndGame = 5,
        Problem = -1
    }
}
