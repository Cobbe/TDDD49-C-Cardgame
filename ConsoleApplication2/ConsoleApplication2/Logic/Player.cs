using GUI;
using GwentStandalone.LINQ;
using GwentStandalone.Logic;
using GwentStandAlone;
using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Logic
{
    [Table(Name = "Player")]
    public class Player
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id;
        [Column]
        public string name;
        [Column]
        public bool ai;
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
            setStrength(strength + card.strength);
            //strength += card.strength;
            getPlayedCards().moveCardHere(card);
        }

        public Player() : base()
        {
            
        }

        public CardHandler getDeck()
        {
            return Storage.getCardHandler(this.id, "deck");
        }

        public CardHandler getHand()
        {
            return Storage.getCardHandler(this.id, "hand");
        }

        public CardHandler getPlayedCards()
        {
            return Storage.getCardHandler(this.id, "playedCards");
        }

        public CardHandler getUsedCards()
        {
            return Storage.getCardHandler(this.id, "usedCards");
        }
        

        public bool determineAndPerformAction()
        {
            if (!RuleEngine.allowedToPlay(this))
            {
                return false;
            }

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
            if (Storage.getPlayer1().id == id)
            {
                opponentStrength = Storage.getPlayer2().strength;
                wins = LogicEngine.getWonBattlesPlayer1();
                opponentPass = Storage.getPlayer2().pass;
            } else
            {
                opponentStrength = Storage.getPlayer1().strength;
                wins = LogicEngine.getWonBattlesPlayer2();
                opponentPass = Storage.getPlayer1().pass;
            }
            

            // Sets the threshold for when it could be wise to pass
            int passThreshold = 10;

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
            Storage.setPass(this.id, pass);
        }

        public void setStrength(int strength)
        {
            Storage.setStrength(this.id, strength);
        }

        public void movePlayedCardsToUsed()
        {
            foreach(Card card in getPlayedCards().getCards())
            {
                getUsedCards().moveCardHere(card);
            }
        }
        
    }
}
