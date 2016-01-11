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
            for (int i = 0; i < number; i++)
            {
                //FIX THIS (randomness)
                getHand().moveCardHere(getDeck().getCards().First());
            }
        }

        public void playCard(Card card)
        {
            //Program.db.ExecuteCommand("UPDATE Player SET strength =2 WHERE id = 119");
            //Console.WriteLine("Old Strength: " + Table.getTableInstance().getPlayer(this.name).strength + " Card strength: " + card.strength);
            setStrength(Table.getTableInstance().getPlayer(this.name).strength + card.strength);
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

        /* Method which controls the AI's actions */
        public void determineAndPerformAction(int opponentStrength, int round, int wins, bool playerPass)
        {
            if (strength > opponentStrength)
            {
                if (playerPass)
                {
                    // AI will pass and win
                    setPass(true);
                }
                else
                {
                    // AI should play a weak card
                    playCard(getWeakestCard());
                }

            }
            else
            {
                if (round == 1)
                {
                    if (strength + 10 > opponentStrength)
                    {
                        // AI should play a card and try to go for the win
                        playCard(getStrongestCard());
                    }
                    else
                    {
                        setPass(true);
                    }
                }
                else if (round == 2)
                {
                    if (wins == 0)
                    {
                        // AI should play a card and try to go for the win
                        playCard(getStrongestCard());
                    }
                    else
                    {
                        if (strength + 10 > opponentStrength)
                        {
                            // AI should play a card and try to go for the win
                            playCard(getStrongestCard());
                        }
                        else
                        {
                            // AI should pass and take the loss
                            setPass(true);
                        }
                    }
                }
                else
                {
                    // AI should play a card and try to go for the win
                    playCard(getStrongestCard());
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
            //Console.WriteLine("Strength: " + strength);
            Program.db.ExecuteCommand("UPDATE Player SET strength ={0} WHERE id = {1}", strength, this.id);
            Program.db.Refresh(RefreshMode.OverwriteCurrentValues, Program.db.GetTable<Player>());
            //Program.db.SubmitChanges();
            //Program.db.ExecuteCommand("UPDATE Player SET strength = 10 WHERE id = 77");
        }
    }
}
