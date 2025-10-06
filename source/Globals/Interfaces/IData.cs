using Globals.Entities;
using System.Collections.Generic;

namespace Globals.Interfaces
{
    public interface IData
    {
        public Stock Stock { get; }
        public Dictionary<string, Pile> Piles { get; }
        public Dictionary<string, Foundation> Foundations { get; }

        public void New();
        public void SaveFile(string filename);
        public void LoadFile(string filename);
        public IEnumerable<string> LoadDirectory();
        public void DeleteFile(string filename);
    }
}
