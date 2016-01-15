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
        // End of round?
        public static bool endOfRound(Player player1, Player player2)
        {
            return player1.pass == true && player2.pass == true;
        }

        // Who won the round?
        public static Player determineRoundWinner(Player player1, Player player2)
        {
            return (player1.strength > player2.strength) ? player1 : player2;
        }

        // Who won the game?
        public static bool isPlayer1Winner(int player1wins, int player2wins)
        {
            return (player1wins > player2wins);
        }

        // End game when someone has 2 wins
        public static bool endGame(int player1wins, int player2wins)
        {
            return player1wins == 2 || player2wins == 2;
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
