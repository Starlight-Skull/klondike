using Globals.Entities;
using Globals.Interfaces;

namespace Globals.Structs
{
    public struct Move : IUndoable
    {
        private readonly Deck _from;
        private readonly Deck _to;
        private readonly int _fromIndex;
        private readonly int _toIndex;

        public Move(Deck from, Deck to)
        {
            _from = from;
            _to = to;
            _fromIndex = _from.LastIndex;
            _toIndex = _to.Count;
            Do();
        }

        public Move(Deck from, Deck to, int index)
        {
            _from = from;
            _to = to;
            _fromIndex = index;
            _toIndex = _to.Count;
            Do();
        }

        public void Do()
        {
            var moved = _from.Move(_fromIndex);
            try
            {
                _to.Add(moved);
            }
            catch (System.Exception)
            {
                _from.AddNoCheck(moved);
                throw;
            }
        }

        public void UnDo()
        {
            _from.AddNoCheck(_to.Move(_toIndex));
        }

        public override string ToString()
        {
            return $"Move:{_from.Name}({_fromIndex})->{_to.Name}({_toIndex})";
        }
    }
}
