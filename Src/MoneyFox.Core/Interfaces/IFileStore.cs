using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MoneyFox.Core.Interfaces
{
    public interface IFileStore
    {
        Task<bool> Exists(string path);

        Task<bool> FolderExists(string path);

        Task<Stream> OpenRead(string path);

        void DeleteFile(string filePath);

        void WriteFile(string path, IEnumerable<byte> contents);
    }
}