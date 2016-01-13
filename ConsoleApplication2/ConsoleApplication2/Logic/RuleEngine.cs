using GwentStandalone.LINQ;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwentStandalone.Logic
{
    class RuleEngine
    {
        public static GameState determineRound()
        {
            if (Storage.getPlayer1().pass == true && Storage.getPlayer2().pass == true)
            {
                if (Storage.getPlayer1().strength > Storage.getPlayer2().strength)
                {
                    LogicEngine.player1Won();
                }
                else
                {
                    LogicEngine.player2Won();
                }
                if ((LogicEngine.getWonBattlesPlayer1() == 2 && LogicEngine.getWonBattlesPlayer2() == 0) || (LogicEngine.getWonBattlesPlayer1() == 0 && LogicEngine.getWonBattlesPlayer2() == 2))
                {
                    // Add an extra round
                    LogicEngine.nextRound();
                }
                LogicEngine.nextRound();
            }
            if (LogicEngine.getRound() > 3)
            {
                LogicEngine.nextRound();
                return GameState.EndGame;
            }

            return GameState.P1Turn;
        }

        // Checks if a player/ai has already passed this round, in that case do not allow it to play this turn.
        public static bool allowedToPlay(Player player)
        {
            if (player.pass)
            {
                return false;
            }
            return true;
        }
    }
}
