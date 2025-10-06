using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Globals.Entities
{
    [Serializable]
    public abstract class Deck
    {
        [JsonInclude]
        public string Name { get; }
        [JsonInclude]
        public List<Card> Cards { get; protected set; }
        [JsonIgnore]
        public int LastIndex => Cards.Count > 0 ? Cards.Count - 1 : 0;
        [JsonIgnore]
        public int Count => Cards.Count;
        [JsonIgnore]
        public abstract bool IsComplete { get; }

        [JsonIgnore]
        public Card this[int index] => Cards[index];

        public event Action CardsChanged;

        public Deck(string name)
        {
            Cards = new List<Card>();
            Name = name;
        }

        public Deck(List<Card> copy, string name)
        {
            Cards = copy;
            Name = name;
        }

        public Deck(Deck copy)
        {
            Cards = copy.Cards;
            Name = copy.Name;
        }

        public virtual void Add(List<Card> cards)
        {
            AddNoCheck(cards);
        }

        public virtual void Add(Card card)
        {
            Cards.Add(card);
        }

        public void AddNoCheck(List<Card> cards)
        {
            foreach (Card card in cards) Cards.Add(card);
            CardsChanged?.Invoke();
        }

        public virtual List<Card> Move(int index)
        {
            if (Cards[index].IsHidden) throw new ArgumentException("You can't move hidden cards.");
            else
            {
                var moved = Cards.Move(index, Count);
                CardsChanged?.Invoke();
                return moved;
            }
        }

        public void Prepend(List<Card> cards)
        {
            cards.AddRange(Cards);
            Cards = cards;
            CardsChanged?.Invoke();
        }

        public void HideLast(bool hide)
        {
            Cards.Last().IsHidden = hide;
            CardsChanged?.Invoke();
        }

        public override string ToString()
        {
            string cardString = "";
            foreach (Card card in Cards) cardString += $"{card.Color}{card.Value} ";
            return cardString;
        }

        public string[] ToStringArray()
        {
            string[] array = new string[Cards.Count];
            for (int i = 0; i < array.Length; i++) array[i] = Cards[i].ToString();
            return array;
        }

        // Removed because of problems with serialization

        //public IEnumerator<Card> GetEnumerator()
        //{
        //    return Cards.GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return Cards.GetEnumerator();
        //}
    }
}
