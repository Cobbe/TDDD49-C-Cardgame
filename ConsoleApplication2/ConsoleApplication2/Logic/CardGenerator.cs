using System;

namespace Logic
{
    class CardGenerator
    {
        private static Random randomGenerator;
        
        public CardGenerator()
        {
            randomGenerator = new Random();
        }

        public Card generateRandomCard(int DeckId)
        {
            switch (randomGenerator.Next(1, 30))
            {
                case 1:
                    return new Card("Weak Orc", "ugh", "warrior_orc.png", randomGenerator.Next(1, 6), DeckId);
                case 2:
                    return new Card("Orc", "wagh", "warrior_orc.png", randomGenerator.Next(6, 8), DeckId);
                case 3:
                    return new Card("Strong Orc", "Waagh!", "warrior_orc.png", randomGenerator.Next(8, 12), DeckId);
                case 4:
                    return new Card("Weak Orc", "ugh", "warrior_orc.png", randomGenerator.Next(1, 6), DeckId);
                case 5:
                    return new Card("Orc", "wagh", "warrior_orc.png", randomGenerator.Next(6, 8), DeckId);
                case 6:
                    return new Card("Fierce Orc", "Waagh!!", "warrior_orc.png", randomGenerator.Next(12, 15), DeckId);
                case 7:
                    return new Card("Dragonling", "Cute", "dragon.png", randomGenerator.Next(1, 3), DeckId);
                case 8:
                    return new Card("Dragonling", "Cute", "dragon.png", randomGenerator.Next(1, 3), DeckId);
                case 9:
                    return new Card("Dragonling", "Cute", "dragon.png", randomGenerator.Next(1, 3), DeckId);
                case 10:
                    return new Card("Dragonling", "Cute", "dragon.png", randomGenerator.Next(1, 3), DeckId);
                case 11:
                    return new Card("Drake", "Bites", "dragon.png", randomGenerator.Next(3, 9), DeckId);
                case 12:
                    return new Card("Orc Champion", "Waaagh!!!", "warrior_orc.png", randomGenerator.Next(15, 21), DeckId);
                case 13:
                    return new Card("Drake", "Bites", "dragon.png", randomGenerator.Next(3, 9), DeckId);
                case 14:
                    return new Card("Orc", "wagh", "warrior_orc.png", randomGenerator.Next(6, 8), DeckId);
                case 15:
                    return new Card("Drake", "Bites", "dragon.png", randomGenerator.Next(3, 9), DeckId);
                case 16:
                    return new Card("Dragon", "Breathes fire", "dragon.png", randomGenerator.Next(21, 30), DeckId);
                case 17:
                    return new Card("Witch Apprentice", "Abra", "witch.png", randomGenerator.Next(1, 10), DeckId);
                case 18:
                    return new Card("Witch", "Dangerous", "witch.png", randomGenerator.Next(12, 15), DeckId);
                case 19:
                    return new Card("Witch Apprentice", "Abra", "witch.png", randomGenerator.Next(1, 10), DeckId);
                case 20:
                    return new Card("Witch Apprentice", "Abra", "witch.png", randomGenerator.Next(1, 10), DeckId);
                case 21:
                    return new Card("Weak Orc", "ugh", "warrior_orc.png", randomGenerator.Next(1, 6), DeckId);
                case 22:
                    return new Card("Weak Orc", "ugh", "warrior_orc.png", randomGenerator.Next(1, 6), DeckId);
                case 23:
                    return new Card("Weak Orc", "ugh", "warrior_orc.png", randomGenerator.Next(1, 6), DeckId);
                case 24:
                    return new Card("Weak Orc", "ugh", "warrior_orc.png", randomGenerator.Next(1, 6), DeckId);
                default:
                    return new Card("Witch Apprentice", "Abra", "witch.png", randomGenerator.Next(1, 10), DeckId);

            }

        }
        
    }
}
