using Globals.Entities;
using Globals.Interfaces;
using Globals.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Logic
{
    public class CardLogic : ILogic
    {
        private readonly IData _cardData;
        private Dictionary<string, bool> _isCompleted;
        private Dictionary<string, bool> _isCompletable;
        private List<IUndoable> _moves;
        private int _selectedIndex;
        private Deck _selected;

        public int Moves => _moves.Count<IUndoable>(u => u.GetType() == typeof(Move));
        public Stock Stock => _cardData.Stock;
        public Dictionary<string, Pile> Piles => _cardData.Piles;
        public Dictionary<string, Foundation> Foundations => _cardData.Foundations;
        public int TotalCards
        {
            get
            {
                int count = Stock.Count;
                foreach (var pile in Piles) count += pile.Value.Count;
                foreach (var pile in Foundations) count += pile.Value.Count;
                return count;
            }
        }

        public event Action<bool> IsCompletedChanged;
        public event Action<bool> IsCompletableChanged;

        public CardLogic(IData data)
        {
            _cardData = data;
            Init();
        }

        private void Init()
        {
            _moves = new List<IUndoable>();
            _isCompleted = new Dictionary<string, bool>();
            _isCompletable = new Dictionary<string, bool> { { Stock.Name, Stock.IsComplete } };
            Stock.CardsChanged += () =>
            {
                _isCompletable["Stock"] = Stock.IsComplete;
                IsCompletableChanged?.Invoke(AllTrue(_isCompletable.Values));
            };
            InitCardTracker(Piles.Values, _isCompletable, IsCompletableChanged);
            InitCardTracker(Foundations.Values, _isCompleted, IsCompletedChanged);
            Unselect();
        }

        private static void InitCardTracker(IEnumerable<Deck> decks, Dictionary<string, bool> bools, Action<bool> eventAction)
        {
            foreach (var deck in decks)
            {
                bools.Add(deck.Name, deck.IsComplete);
                deck.CardsChanged += () =>
                {
                    bools[deck.Name] = deck.IsComplete;
                    eventAction?.Invoke(AllTrue(bools.Values));
                };
            }
        }

        private static bool AllTrue(IEnumerable<bool> list)
        {
            bool allOk = true;
            foreach (var item in list) allOk &= item;
            return allOk;
        }

        public void Unselect()
        {
            _selected = null;
            _selectedIndex = -1;
        }

        public void SelectCard(Deck deck, int index)
        {
            if (index != -1)
            {
                if (deck[index].IsHidden) throw new ArgumentException("Card is unknown.");
                _selected = deck;
                _selectedIndex = index;
            }
        }

        public void MoveTo(Deck deck)
        {
            if (_selected == null || _selectedIndex == -1) throw new ArgumentException("Nothing to move.");
            if (_selected == deck && _selectedIndex == _selected.LastIndex)
            {
                _moves.Add(new Move(_selected, Foundations[_selected[^1].Color.ToString()]));
                if (_selected.Count > 0 && _selected[^1].IsHidden) _moves.Add(new Reveal(_selected, _selected.LastIndex));
            }
            else
            {
                _moves.Add(new Move(_selected, deck, _selectedIndex));
                if (_selected.Count > 0 && _selected[^1].IsHidden) _moves.Add(new Reveal(_selected, _selected.LastIndex));
            }
            Unselect();
        }

        public void DrawCard()
        {
            Unselect();
            _moves.Add(new Draw(Stock));
        }

        public void UndoLast()
        {
            Unselect();
            if (_moves.Count > 0)
            {
                var loop = (_moves[^1].GetType() == typeof(Reveal));
                _moves[^1].UnDo();
                _moves.RemoveAt(_moves.Count - 1);
                if (loop) UndoLast();
            }
            else throw new ArgumentException("Nothing to undo.");
        }

        public void AutoSolve()
        {
            if (!AllTrue(_isCompletable.Values)) throw new ArgumentException("All cards must be visible and not in Stock to Auto-Solve.");
            if (AllTrue(_isCompleted.Values)) return;
            Deck smallest = new Stock(new List<Card> { new Card(Globals.Enums.CardColor.Spades, Globals.Enums.CardValue.King) });
            foreach (var pile in Piles.Values)
            {
                if (pile.Count == 0) continue;
                if (pile[pile.LastIndex] <= smallest[smallest.LastIndex]) smallest = pile;
            }
            Foundations[smallest[^1].Color.ToString()].Add(smallest.Move(smallest.LastIndex));
            Thread.Sleep(300);
            AutoSolve();
        }

        public void SaveFile(string filename)
        {
            Unselect();
            _cardData.SaveFile(filename);
        }

        public void LoadFile(string filename)
        {
            _cardData.LoadFile(filename);
            Init();
        }

        public IEnumerable<string> LoadDirectory()
        {
            Unselect();
            return _cardData.LoadDirectory();
        }

        public void New()
        {
            _cardData.New();
            Init();
        }

        public void DeleteFile(string filename)
        {
            Unselect();
            _cardData.DeleteFile(filename);
        }
    }
}
