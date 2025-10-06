using Globals.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Globals.Entities
{
    [Serializable]
    public class Pile : Deck
    {
        [JsonIgnore]
        public override bool IsComplete => !Cards.Any((card) => card.IsHidden);

        public Pile(List<Card> cards, int count) : base($"Pile {count}")
        {
            Cards.AddRange(cards.Move(0, count));
            for (int i = 0; i < LastIndex; i++) Cards[i].IsHidden = true;
        }

        [JsonConstructor]
        public Pile(List<Card> cards, string name) : base(cards, name) { }

        public override void Add(List<Card> newCards)
        {
            bool allOk = true;
            for (int i = 0; i < newCards.Count; i++)
            {
                if (newCards[i].IsHidden) throw new ArgumentException("You can't move unknown cards.");
                if (i == 0)
                {
                    if (Cards.Count == 0)
                    {
                        if (newCards.First().Value != CardValue.King) throw new ArgumentException($"First card must be {CardValue.King}.");
                    }
                    else if (!Check(newCards[i], Cards[LastIndex])) allOk = false;
                }
                else if (!Check(newCards[i], newCards[i - 1])) allOk = false;
            }
            if (allOk) base.Add(newCards);
            else throw new ArgumentException("The order must be ascending and changing color.");
        }

        private static bool Check(Card current, Card previous)
        {
            return current.Value == previous.Value - 1
                && (current.Color is CardColor.Clubs or CardColor.Spades && previous.Color is CardColor.Diamonds or CardColor.Hearts
                || current.Color is CardColor.Diamonds or CardColor.Hearts && previous.Color is CardColor.Clubs or CardColor.Spades);
        }
    }
}
