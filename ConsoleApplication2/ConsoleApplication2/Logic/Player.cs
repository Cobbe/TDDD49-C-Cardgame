using GUI;
using GwentStandAlone;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Logic
{
    [Table(Name = "Player")]
    class Player
    {
        [Column(IsPrimaryKey = true)]
        public int id;
        [Column]
        public string name;
        [Column]
        private bool ai;
        [Column]
        public int strength;
        [Column]
        public bool pass;

        public void drawCards(int number)
        {
            Random rnd = new Random();
            for (int i = 0; i < number; i++)
            {
                int cardNumber = rnd.Next(0, getDeck().numberOFCards());
                getHand().moveCardHere(getDeck().getCards()[cardNumber]);
            }
        }

        public void playCard(Card card)
        {
            //Program.db.ExecuteCommand("UPDATE Player SET strength =2 WHERE id = 119");
            //Console.WriteLine("Old Strength: " + Table.getTableInstance().getPlayer(this.name).strength + " Card strength: " + card.strength);
            setStrength(getDBInstance().strength + card.strength);
            //strength += card.strength;
            getPlayedCards().moveCardHere(card);
        }

        public Player() : base()
        {

        }

        private CardHandler getCardHandler(String type)
        {
            Table<CardHandler> cardHandlers = Program.db.GetTable<CardHandler>();
            var query = from cardHandler in cardHandlers
                        where cardHandler.type == type &&
                        cardHandler.playerId == this.id
                        select cardHandler;
            CardHandler res = null;
            foreach (var cardHandler in query)
                res = cardHandler;

            return res;
        }

        public CardHandler getDeck()
        {
            return getCardHandler("deck");
        }

        public CardHandler getHand()
        {
            return getCardHandler("hand");
        }

        public CardHandler getPlayedCards()
        {
            return getCardHandler("playedCards");
        }

        public CardHandler getUsedCards()
        {
            return getCardHandler("usedCards");
        }

        public bool getAI()
        {
            Player temp = getDBInstance();
            if (temp == null)
            {
                return false;
            }
            return getDBInstance().ai;
        }

        public bool determineAndPerformAction()
        {
            if (ai)
            {
               return aiControls();
            } else
            {
                return playerControls();
            }
        }

        public bool playerControls()
        {
            if(getDBInstance().pass == true)
            {
                return false;
            }
            if (GameForm.activePass)
            {
                setPass(true);
                GameForm.activePass = false;
                return false;
            } else if(GameForm.activeClick)
            {
                playCard(GameForm.lastClickedCard);
                GameForm.activeClick = false;
                return false;
            }
            return true;
        }

        /* Method which controls the AI's actions */
        public bool aiControls()
        {
            bool opponentPass;
            int opponentStrength;
            int round = LogicEngine.getRound();
            int wins;
            if (LogicEngine.getPlayer1().id == id)
            {
                opponentStrength = LogicEngine.getPlayer2().strength;
                wins = LogicEngine.getWonBattlesPlayer1();
                opponentPass = LogicEngine.getPlayer2().pass;
            } else
            {
                opponentStrength = LogicEngine.getPlayer1().strength;
                wins = LogicEngine.getWonBattlesPlayer2();
                opponentPass = LogicEngine.getPlayer1().pass;
            }
            

            // Sets the threshold for when it could be wise to pass
            int passThreshold = 5;
            Console.WriteLine("opponentPass: " + opponentPass);
            if(getHand().numberOFCards() == 0)
            {
                // No cards left, AI must pass
                setPass(true);
                return false;
            }
            if (strength > opponentStrength)
            {
                if (opponentPass)
                {
                    // AI will pass and win
                    setPass(true);
                    return false;
                }
                else
                {
                    // AI should play a weak card
                    playCard(getWeakestCard());
                    return false;
                }

            }
            else
            {
                if (round == 1)
                {
                    if (strength + passThreshold > opponentStrength)
                    {
                        // AI should play a card and try to go for the win
                        playCard(getStrongestCard());
                        return false;
                    }
                    else
                    {
                        setPass(true);
                        return false;
                    }
                }
                else if (round == 2)
                {
                    if (wins == 0)
                    {
                        // AI should play a card and try to go for the win
                        playCard(getStrongestCard());
                        return false;
                    }
                    else
                    {
                        if (strength + passThreshold > opponentStrength)
                        {
                            // AI should play a card and try to go for the win
                            playCard(getStrongestCard());
                            return false;
                        }
                        else
                        {
                            // AI should pass and take the loss
                            setPass(true);
                            return false;
                        }
                    }
                }
                else
                {
                    // AI should play a card and try to go for the win
                    playCard(getStrongestCard());
                    return false;
                }
            }
        }

        /* Retrieves the card with the highest strength from the hand */
        public Card getStrongestCard()
        {
            return getHand().getCards().OrderByDescending(card => card.strength).First();
        }

        /* Retrieves the card with the lowest strength from the hand */
        public Card getWeakestCard()
        {
            return getHand().getCards().OrderBy(card => card.strength).First();
        }

        public void setPass(bool pass)
        {
            Program.db.ExecuteCommand("UPDATE Player SET pass ={0} WHERE id = {1}", pass, this.id);
            Program.db.Refresh(RefreshMode.OverwriteCurrentValues, Program.db.GetTable<Player>());
        }

        public void setStrength(int strength)
        {
            Program.db.ExecuteCommand("UPDATE Player SET strength ={0} WHERE id = {1}", strength, this.id);
            Program.db.Refresh(RefreshMode.OverwriteCurrentValues, Program.db.GetTable<Player>());
        }

        public void movePlayedCardsToUsed()
        {
            foreach(Card card in getPlayedCards().getCards())
            {
                getUsedCards().moveCardHere(card);
            }
        }

        // Retrieves the DB instance of this object
        private Player getDBInstance()
        {
            Table<Player> players = Program.db.GetTable<Player>();
            var query = from player in players where player.id == this.id select player;

            return query.First();
        }
    }
}
