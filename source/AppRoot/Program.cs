using Data;
using KlondikeCLI;
using Logic;

namespace AppRoot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // IData data = new CardData();
            // ILogic logic = new CardLogic(data);
            // new ConsoleUI(logic).Run();
            new ConsoleUI(new CardLogic(new CardData())).Run();
        }
    }
}
