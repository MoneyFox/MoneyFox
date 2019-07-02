using System.Collections.Generic;
using System.IO;

namespace MoneyFox.BusinessLogic.FileStore
{
    public interface IFileStore
    {
        void WriteFile(string path, IEnumerable<byte> contents);

        bool TryMove(string from, string destination, bool overwrite);

        Stream OpenRead(string path);
    }
}
