using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoneyFox.Application.Common.FileStore
{
    public interface IFileStore
    {
        Task WriteFileAsync(string path, IEnumerable<byte> contents);

        Task<bool> TryMoveAsync(string from, string destination, bool overwrite);

        Task<Stream> OpenReadAsync(string path);
    }
}