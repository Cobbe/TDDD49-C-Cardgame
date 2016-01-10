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
        public Hand hand;
        public Deck deck;
        public PlayedCards playedCards;
        private UsedCards usedCards;

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
                hand.addCard(deck.drawCard());
            }
        }

        public void playCard(Card card)
        {
                strength += card.strength;
                playedCards.addCard(card);

        }

        public Player()
        {
            deck = new Deck();
            hand = new Hand();
            playedCards = new PlayedCards();
            usedCards = new UsedCards();
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
        public void determineAndPerformAction(int opponentStrength, int round, int wins, bool playerPass)
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
                        playCard(hand.getCard(getWeakestCard()));
                    }
                    
                } else
                {
                    if(round == 1)
                    {
                        if(strength+10 > opponentStrength)
                        {
                            // AI should play a card and try to go for the win
                            playCard(hand.getCard(getStrongestCard()));
                        } else
                        {
                            pass = true;
                        }
                    } else if(round == 2)
                    {
                        if(wins == 0)
                        {
                            // AI should play a card and try to go for the win
                            playCard(hand.getCard(getStrongestCard()));
                        } else
                        {
                            if(strength+10 > opponentStrength)
                            {
                                // AI should play a card and try to go for the win
                                playCard(hand.getCard(getStrongestCard()));
                            } else
                            {
                                // AI should pass and take the loss
                                pass = true;
                            }
                        }
                    } else
                    {
                        // AI should play a card and try to go for the win
                        playCard(hand.getCard(getStrongestCard()));
                    }
                }
        }

        /* Retrieves the index of the monster card with the highest strength, if there are no monster cards in the deck it sends back -1 */
        public int getStrongestCard()
        {
            int indexOfHigh = -1;
            int strongest = -1;
            for (int i = 0; i < hand.numberOFCards(); i++)
            {
                    int testCardStrength = hand.viewCard(i).strength;
                    if (testCardStrength > strongest)
                    {
                        strongest = testCardStrength;
                        indexOfHigh = i;
                    }
            }
            return indexOfHigh;
        }

        /* Retrieves the index of the monster card with the lowest strength, if there are no monster cards in the deck it sends back -1 */
        public int getWeakestCard()
        {
            int indexOfHigh = -1;
            int weakest = 1000;
            for (int i = 0; i < hand.numberOFCards(); i++)
            {
                    int testCardStrength = hand.viewCard(i).strength;
                    if (testCardStrength < weakest)
                    {
                        weakest = testCardStrength;
                        indexOfHigh = i;
                    }
            }
            return indexOfHigh;
        }
    }
}
