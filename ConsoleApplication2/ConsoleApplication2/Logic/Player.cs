using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private int strength;
        private bool pass = false;
        
        public int Strength
        {
            get
            {
                return strength;
            }

            set
            {
                strength = value;
            }
        }

        public bool Pass
        {
            get
            {
                return pass;
            }

            set
            {
                pass = value;
            }
        }

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

        public Player()
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
                if(Strength > opponentStrength)
                {
                    if (playerPass)
                    {
                        // AI will pass and win
                        Pass = true;
                    } else
                    {
                        // AI should play a weak card
                        playCard(getWeakestCard(), db);
                    }
                    
                } else
                {
                    if(round == 1)
                    {
                        if(Strength+10 > opponentStrength)
                        {
                            // AI should play a card and try to go for the win
                            playCard(Hand.getCard(getStrongestCard()));
                        } else
                        {
                            Pass = true;
                        }
                    } else if(round == 2)
                    {
                        if(wins == 0)
                        {
                            // AI should play a card and try to go for the win
                            playCard(Hand.getCard(getStrongestCard()));
                        } else
                        {
                            if(Strength+10 > opponentStrength)
                            {
                                // AI should play a card and try to go for the win
                                playCard(Hand.getCard(getStrongestCard()));
                            } else
                            {
                                // AI should pass and take the loss
                                Pass = true;
                            }
                        }
                    } else
                    {
                        // AI should play a card and try to go for the win
                        playCard(Hand.getCard(getStrongestCard()));
                    }
                }
        }

        /* Retrieves the index of the monster card with the highest strength, if there are no monster cards in the deck it sends back -1 */
        public int getStrongestCard()
        {
            int indexOfHigh = -1;
            int strongest = -1;
            for (int i = 0; i < Hand.numberOFCards(); i++)
            {
                if (Hand.viewCard(i) is MonsterCard)
                {
                    int testCardStrength = ((MonsterCard)Hand.viewCard(i)).Strength;
                    if (testCardStrength > strongest)
                    {
                        strongest = testCardStrength;
                        indexOfHigh = i;
                    }
                }
            }
            return indexOfHigh;
        }

        /* Retrieves the index of the monster card with the lowest strength, if there are no monster cards in the deck it sends back -1 */
        public int getWeakestCard()
        {
            int indexOfHigh = -1;
            int weakest = 1000;
            for (int i = 0; i < Hand.numberOFCards(); i++)
            {
                if (Hand.viewCard(i) is MonsterCard)
                {
                    int testCardStrength = ((MonsterCard)Hand.viewCard(i)).Strength;
                    if (testCardStrength < weakest)
                    {
                        weakest = testCardStrength;
                        indexOfHigh = i;
                    }
                }
            }
            return indexOfHigh;
        }
    }
}
