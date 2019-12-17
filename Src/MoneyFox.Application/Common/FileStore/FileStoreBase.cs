using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoneyFox.Application.Common.FileStore
{
    public abstract class FileStoreBase : IFileStore
    {
        public void WriteFile(string path, IEnumerable<byte> contents)
        {
            WriteFileCommon(path, stream =>
            {
                using (var binaryWriter = new BinaryWriter(stream))
                {
                    binaryWriter.Write(contents.ToArray());
                    binaryWriter.Flush();
                }
            });
        }

        public abstract Stream OpenRead(string path);

        public abstract bool TryMove(string from, string destination, bool overwrite);

        protected abstract void WriteFileCommon(string path, Action<Stream> streamAction);
    }
}
