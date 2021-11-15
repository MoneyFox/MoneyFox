﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Application.Common.FileStore
{
    public abstract class FileStoreBase : IFileStore
    {
        public async Task WriteFileAsync(string path, IEnumerable<byte> contents) =>
            await WriteFileCommonAsync(
                path,
                stream =>
                {
                    using var binaryWriter = new BinaryWriter(stream);
                    binaryWriter.Write(contents.ToArray());
                    binaryWriter.Flush();
                });

        public abstract Task<Stream> OpenReadAsync(string path);

        public abstract Task<bool> TryMoveAsync(string from, string destination, bool overwrite);

        protected abstract Task WriteFileCommonAsync(string path, Action<Stream> streamAction);
    }
}