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
        
        public int strength;
        public bool pass = false;

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
            strength += card.strength;
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
                    pass = true;
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
                        pass = true;
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
                            pass = true;
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
    }
}
