using Globals.Entities;
using Globals.Interfaces;

namespace Globals.Structs
{
    public struct Draw : IUndoable
    {
        private Stock _stock;

        public Draw(Stock stock)
        {
            _stock = stock;
            Do();
        }

        public void Do()
        {
            _stock.AddNoCheck(_stock.Cards.Move(0, 1));
        }

        public void UnDo()
        {
            _stock.Prepend(_stock.Move(_stock.LastIndex));
        }

        public override string ToString()
        {
            return "Draw";
        }
    }
}
