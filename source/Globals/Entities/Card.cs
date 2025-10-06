using Globals.Enums;
using System;
using System.Text.Json.Serialization;

namespace Globals.Entities
{
    [Serializable]
    public class Card : IComparable<Card>
    {
        public CardColor Color { get; }
        public CardValue Value { get; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsHidden { get; set; }

        public Card(CardColor color, CardValue value)
        {
            Color = color;
            Value = value;
            IsHidden = default;
        }

        [JsonConstructor]
        public Card(CardColor color, CardValue value, bool isHidden)
        {
            Color = color;
            Value = value;
            IsHidden = isHidden;
        }

        public Card(Card card)
        {
            Color = card.Color;
            Value = card.Value;
            IsHidden = card.IsHidden;
        }

        public override string ToString()
        {
            if (IsHidden) return "--";
            char val = (int)Value switch
            {
                1 => 'A',
                10 => 'T',
                11 => 'J',
                12 => 'Q',
                13 => 'K',
                _ => (char)(Value + '0')
            };
            return $"{(char)Color}{val}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Card card)
            {
                return Color == card.Color && Value == card.Value;
            }
            else return false;
        }

        public int CompareTo(Card other)
        {
            return Value.CompareTo(other.Value);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(Card left, Card right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Card left, Card right)
        {
            return !(left == right);
        }

        public static bool operator <(Card left, Card right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Card left, Card right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Card left, Card right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Card left, Card right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
