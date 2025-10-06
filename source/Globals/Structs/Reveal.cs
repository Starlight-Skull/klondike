using Globals.Entities;
using Globals.Interfaces;

namespace Globals.Structs
{
    public struct Reveal : IUndoable
    {
        private readonly Deck _deck;
        private readonly int _index;
        private readonly bool _hidden;

        public Reveal(Deck deck, int index)
        {
            _deck = deck;
            _index = index;
            _hidden = _deck[_index].IsHidden;
            Do();
        }

        public void Do()
        {
            _deck.HideLast(false);
        }

        public void UnDo()
        {
            _deck.HideLast(_hidden);
        }

        public override string ToString()
        {
            return $"Reveal:{_deck[_index]}";
        }
    }
}
