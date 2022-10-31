namespace MoneyFox.Core.Interfaces;

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public interface IFileStore
{
    Task WriteFileAsync(string path, IEnumerable<byte> contents);

    Task<bool> TryMoveAsync(string from, string destination, bool overwrite);

    Task<Stream> OpenReadAsync(string path);
}
