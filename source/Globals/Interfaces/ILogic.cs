using Globals.Entities;
using System;

namespace Globals.Interfaces
{
    public interface ILogic : IData
    {
        public int Moves { get; }
        public int TotalCards { get; }

        public event Action<bool> IsCompletedChanged;
        public event Action<bool> IsCompletableChanged;

        public void SelectCard(Deck deck, int index);
        public void Unselect();
        public void DrawCard();
        public void MoveTo(Deck deck);
        public void UndoLast();
        public void AutoSolve();
    }
}
