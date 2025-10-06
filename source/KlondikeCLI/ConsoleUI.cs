using Globals.Entities;
using Globals.Enums;
using Globals.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace KlondikeCLI
{
    public class ConsoleUI
    {
        private readonly ILogic _logic;
        private readonly Dictionary<string, int> _posistions = new()
        {
            { "Stock", 4 },
            { "Clubs", 7 },
            { "Diamonds", 8 },
            { "Hearts", 9 },
            { "Spades", 10 },
            { "Pile 1", 13 },
            { "Pile 2", 14 },
            { "Pile 3", 15 },
            { "Pile 4", 16 },
            { "Pile 5", 17 },
            { "Pile 6", 18 },
            { "Pile 7", 19 },
            { "Error", 21 },
            { "Saves", 23 },
            { "Input", 25 },
            { "Help", 27 },
        };

        public ConsoleUI(ILogic logic)
        {
            _logic = logic;
            Console.Title = "Klondike";
            Console.OutputEncoding = Encoding.GetEncoding("utf-8");
        }

        public void Run()
        {
            string[] command;
            string[] previous = { "help" };
            Init();
            do
            {
                command = Console.ReadLine().Trim().ToLower().Split(' ');
                try
                {
                    if (command[0] != "")
                    {
                        GetCommand(command[0]).Invoke(command);
                        previous = command;
                    }
                    else GetCommand(previous[0]).Invoke(previous);
                    ClearLine((24, _posistions["Error"]));
                }
                catch (Exception ex)
                {
                    _logic.Unselect();
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteAt((8, _posistions["Error"]), $"Error:\t\t{ex.Message}");
                }
                finally
                {
                    Console.ResetColor();
                    WriteAt((64, 1), _logic.Moves.ToString());
                    ClearLine((10, _posistions["Input"]));
                }
            } while (command[0] != Command.exit.ToString());
        }

        private void Init()
        {
            _logic.Stock.CardsChanged += () => PrintStock();
            foreach (var pile in _logic.Piles.Values) pile.CardsChanged += () => PrintDeck(pile);
            foreach (var foundation in _logic.Foundations.Values) foundation.CardsChanged += () => PrintDeck(foundation);
            _logic.IsCompletableChanged += OnIsCompletableChanged;
            _logic.IsCompletedChanged += OnIsCompletedChanged;
            Update();
        }

        private void OnIsCompletableChanged(bool isCompletable)
        {
            if (isCompletable)
            {
                _logic.IsCompletableChanged -= OnIsCompletableChanged;
                WriteAt((24, _posistions["Stock"]), "Type \"autosolve\" to complete the deck.");
            };
        }

        private void OnIsCompletedChanged(bool isCompleted)
        {
            if (isCompleted)
            {
                _logic.IsCompletedChanged -= OnIsCompletedChanged;
                Console.Beep(1500, 180);
                Console.Beep(1500, 120);
                Console.Beep(2000, 600);
                WriteAt((24, _posistions["Error"]), "Victory!!");
            }
        }

        private Action<string[]> GetCommand(string command)
        {
            try
            {
                return (Command)Enum.Parse(typeof(Command), command) switch
                {
                    Command.draw or Command.d => (_) => _logic.DrawCard(),
                    Command.move or Command.m => (s) => Move(RequireArgCount(s, 1)),
                    Command.help => Help,
                    Command.undo or Command.u => (_) => _logic.UndoLast(),
                    Command.save => (s) => UpdateSaves(_logic.SaveFile, s),
                    Command.delete => (s) => UpdateSaves(_logic.DeleteFile, s),
                    Command.restart => (_) => ReInit(_logic.New),
                    Command.load => (s) => ReInit(() => _logic.LoadFile(RequireArgCount(s, 1)[1])),
                    Command.autosolve => (_) => _logic.AutoSolve(),
                    _ => (_) => { },
                };
            }
            catch { throw new ArgumentException("Unknown command. Try typing \"help\" for a list of commands."); }
        }

        private void ReInit(Action action)
        {
            action.Invoke();
            Init();
        }

        private void UpdateSaves(Action<string> action, string[] command)
        {
            action.Invoke(RequireArgCount(command, 1)[1]);
            WriteAt((24, _posistions["Saves"]), $"{string.Join(", ", _logic.LoadDirectory())}");
        }

        private static string[] RequireArgCount(string[] command, int count)
        {
            if (command.Length - 1 >= count) return command;
            else throw new ArgumentException("Invalid number of arguments.");
        }

        private void Move(string[] command)
        {
            Deck deck1;
            Deck deck2;
            int index;
            switch (command.Length)
            {
                case 2:
                    deck1 = GetDeck(command[1]);
                    _logic.SelectCard(deck1, deck1.LastIndex);
                    _logic.MoveTo(deck1);
                    break;
                case 3:
                    deck1 = GetDeck(command[1]);
                    deck2 = GetDeck(command[2]);
                    _logic.SelectCard(deck1, deck1.LastIndex);
                    _logic.MoveTo(deck2);
                    break;
                case 4:
                    deck1 = GetDeck(command[1]);
                    deck2 = GetDeck(command[3]);
                    index = int.Parse(command[2]);
                    _logic.SelectCard(deck1, index);
                    _logic.MoveTo(deck2);
                    break;
                default:
                    throw new ArgumentException("Too many arguments.");
            }
        }

        private Deck GetDeck(string deck)
        {
            return deck.ToLower() switch
            {
                "stock" or "s" => _logic.Stock,
                "clubs" => _logic.Foundations["Clubs"],
                "hearts" => _logic.Foundations["Hearts"],
                "diamonds" => _logic.Foundations["Diamonds"],
                "spades" => _logic.Foundations["Spades"],
                "pile1" or "1" => _logic.Piles["Pile 1"],
                "pile2" or "2" => _logic.Piles["Pile 2"],
                "pile3" or "3" => _logic.Piles["Pile 3"],
                "pile4" or "4" => _logic.Piles["Pile 4"],
                "pile5" or "5" => _logic.Piles["Pile 5"],
                "pile6" or "6" => _logic.Piles["Pile 6"],
                "pile7" or "7" => _logic.Piles["Pile 7"],
                _ => throw new ArgumentException("Invalid deck name."),
            };
        }

        private void Help(string[] command)
        {
            string helpText;
            if (command.Length == 1) helpText = string.Join(", ", Enum.GetNames(typeof(Command)));
            else helpText = (Command)Enum.Parse(typeof(Command), command[1]) switch
            {
                Command.help => "help {command}\t\t\t\tDisplays help about {command} or lists available commands.",
                Command.draw or Command.d => "d || draw\t\t\t\tDraws a card from the stock.",
                Command.move or Command.m => "m || move [from] {index} {to}\t\tMoves the card(s) at {index || last index} from [from] to {to || foundation}.",
                Command.undo or Command.u => "u || undo\t\t\t\tReverts the last action.",
                Command.save => "save [filename]\t\t\t\tSaves the current state of the deck. Moves are not saved.",
                Command.delete => "delete [filename]\t\t\tDeletes [filename] from the saves directory.",
                Command.restart => "restart\t\t\t\t\tGenerates a new deck.",
                Command.load => "load [filename]\t\t\t\tRestores a previously saved deck. Moves are not saved.",
                Command.autosolve => "autosolve\t\t\t\tAutomaticaly solves the deck if all cards are revealed and not in Stock.",
                Command.exit => "exit\t\t\t\t\tExits the program.",
                _ => throw new ArgumentException("Unknown command. Try typing \"help\" for a list of commands."),
            };
            Console.ForegroundColor = ConsoleColor.Magenta;
            WriteAt((8, _posistions["Help"]), helpText);
            Console.ResetColor();
        }

        private void Update()
        {
            Console.Clear();
            Console.Write($"\n\t\t\t\t----  Klondike  ----\t Moves:");
            WriteAt((64, 1), _logic.Moves.ToString());
            Console.WriteLine("\n\t--- Stock ---");
            PrintStock();
            Console.WriteLine("\n\t--- Foundations ---");
            foreach (var foundation in _logic.Foundations)
            {
                Console.ForegroundColor = CardToConsoleColor(foundation.Value.Color);
                Console.Write($"\t{foundation.Value.Color} ({(char)foundation.Value.Color}):");
                PrintDeck(foundation.Value);
                Console.ResetColor();
            }
            Console.WriteLine("\n\t--- Piles ---\t  0    1    2    3    4    5    6    7    8    9    10   11   12   13   14   15");
            foreach (var pile in _logic.Piles)
            {
                Console.Write($"\t{pile.Key}:");
                PrintDeck(pile.Value);
                Console.ResetColor();
            }
            Console.Write("\n\n\n\tSaved games:");
            WriteAt((24, _posistions["Saves"]), $"{string.Join(", ", _logic.LoadDirectory())}");
            Console.Write("\n\t> ");
        }

        private static void ClearLine((int left, int top) position)
        {
            Console.SetCursorPosition(position.left, position.top);
            Console.Write(new string(' ', Console.BufferWidth - Console.CursorLeft));
            Console.SetCursorPosition(position.left, position.top);
        }

        private static void WriteAt((int left, int top) position, string text)
        {
            ClearLine(position);
            Console.Write(text);
            Console.WriteLine();
        }

        private void PrintDeck(Deck deck)
        {
            ClearLine((24, _posistions[deck.Name]));
            foreach (Card card in deck.Cards)
            {
                Console.ForegroundColor = (card.IsHidden ? ConsoleColor.Green : CardToConsoleColor(card.Color));
                Console.Write($"[{card}] ");
            }
            Console.WriteLine();
        }

        private void PrintStock()
        {
            ClearLine((8, _posistions[_logic.Stock.Name]));
            if (_logic.Stock.Count < 3) _logic.Stock.ShownCount = _logic.Stock.Count;
            else _logic.Stock.ShownCount = 3;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[--] x{_logic.Stock.Count - _logic.Stock.ShownCount}\t");
            for (int i = _logic.Stock.Count - _logic.Stock.ShownCount; i < _logic.Stock.Count; i++)
            {
                Console.ForegroundColor = CardToConsoleColor(_logic.Stock[i].Color);
                Console.Write($"[{_logic.Stock[i]}] ");
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        private static ConsoleColor CardToConsoleColor(CardColor color)
        {
            return color switch
            {
                CardColor.Clubs or CardColor.Spades => ConsoleColor.Cyan,
                CardColor.Hearts or CardColor.Diamonds => ConsoleColor.Red,
                _ => ConsoleColor.Green,
            };
        }
    }
}
