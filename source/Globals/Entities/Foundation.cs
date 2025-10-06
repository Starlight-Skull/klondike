using Globals.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Globals.Entities
{
    [Serializable]
    public class Foundation : Deck
    {
        [JsonInclude]
        public CardColor Color { get; }
        [JsonIgnore]
        public override bool IsComplete => Cards.Count == 13;

        public Foundation(CardColor color) : base(color.ToString())
        {
            Color = color;
        }

        [JsonConstructor]
        public Foundation(List<Card> cards, string name, CardColor color) : base(cards, name)
        {
            Color = color;
        }

        public override void Add(List<Card> cards)
        {
            if (cards.Count > 1) throw new ArgumentException("Only one card can be added.");
            if (cards.First().IsHidden) throw new ArgumentException("You can't move unknown cards.");
            if (cards.First().Color != Color) throw new ArgumentException($"Card must be {Color}");
            if (Cards.Count == 0 && cards.First().Value != CardValue.Ace) throw new ArgumentException($"First card must be {CardValue.Ace}.");
            if (Cards.Count != 0 && cards.First().Value != Cards.Last().Value + 1) throw new ArgumentException("Card must be bigger than the last card.");
            base.Add(cards);
        }

        public override List<Card> Move(int index)
        {
            if (index != LastIndex) throw new ArgumentException("Only the last card can be moved.");
            return base.Move(index);
        }
    }
}
