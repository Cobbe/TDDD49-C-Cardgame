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

        public void drawCards(int number, DataContext db)
        {
            for (int i = 0; i < number; i++)
            {
                //FIX THIS (randomness)
                getHand(db).moveCardHere(getDeck(db).getCards(db)[0], db);
            }
        }

        public void playCard(Card card, DataContext db)
        {
            strength += card.strength;
            getPlayedCards(db).moveCardHere(card, db);
        }

        public Player() : base()
        {

        }

        private CardHandler getCardHandler(DataContext db, String type)
        {
            Table<CardHandler> cardHandlers = db.GetTable<CardHandler>();
            var query = from cardHandler in cardHandlers
                        where cardHandler.type == type &&
                        cardHandler.playerId == this.id
                        select cardHandler;
            CardHandler res = null;
            foreach (var cardHandler in query)
                res = cardHandler;

            return res;
        }

        public CardHandler getDeck(DataContext db)
        {
            return getCardHandler(db, "deck");
        }

        public CardHandler getHand(DataContext db)
        {
            return getCardHandler(db, "hand");
        }

        public CardHandler getPlayedCards(DataContext db)
        {
            return getCardHandler(db, "playedCards");
        }

        public CardHandler getUsedCards(DataContext db)
        {
            return getCardHandler(db, "usedCards");
        }
    }

    class AI : Player
    {
        public AI() : base()
        {

        }

        /* Method which controls the AI's actions */
        public void determineAndPerformAction(int opponentStrength, int round, int wins, bool playerPass, DataContext db)
        {
                if(strength > opponentStrength)
                {
                    if (playerPass)
                    {
                        // AI will pass and win
                        pass = true;
                    } else
                    {
                        // AI should play a weak card
                        playCard(getWeakestCard(db), db);
                    }
                    
                } else
                {
                    if(round == 1)
                    {
                        if(strength+10 > opponentStrength)
                        {
                            // AI should play a card and try to go for the win
                            playCard(getStrongestCard(db), db);
                        } else
                        {
                            pass = true;
                        }
                    } else if(round == 2)
                    {
                        if(wins == 0)
                        {
                        // AI should play a card and try to go for the win
                        playCard(getStrongestCard(db), db);
                    } else
                        {
                            if(strength+10 > opponentStrength)
                            {
                            // AI should play a card and try to go for the win
                            playCard(getStrongestCard(db), db);
                        } else
                            {
                                // AI should pass and take the loss
                                pass = true;
                            }
                        }
                    } else
                    {
                    // AI should play a card and try to go for the win
                    playCard(getStrongestCard(db), db);
                    }
                }
        }

        /* Retrieves the card with the highest strength from the hand */
        public Card getStrongestCard(DataContext db)
        {
            return getHand(db).getCards(db).OrderByDescending(card => card.strength).First();
        }

        /* Retrieves the card with the lowest strength from the hand */
        public Card getWeakestCard(DataContext db)
        {
            return getHand(db).getCards(db).OrderBy(card => card.strength).First();
        }
    }
}
