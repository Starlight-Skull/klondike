using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Globals.Entities
{
    [Serializable]
    public class Stock : Deck
    {
        [JsonIgnore]
        public override bool IsComplete => Cards.Count == 0;
        [JsonIgnore]
        public int ShownCount { get; set; }

        public Stock(List<Card> cards) : base(cards, "Stock")
        {
            ShownCount = 3;
        }

        [JsonConstructor]
        public Stock(List<Card> cards, string name) : base(cards, name)
        {
            ShownCount = 3;
        }

        public override void Add(List<Card> newCards)
        {
            throw new ArgumentException("Cards cannot be added to Stock.");
        }

        public override List<Card> Move(int index)
        {
            if (index != LastIndex) throw new ArgumentException("Only the last card can be moved.");
            return base.Move(index);
        }
    }
}
