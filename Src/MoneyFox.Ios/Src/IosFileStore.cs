using System;

namespace MoneyFox.iOS
{
    public class IosFileStore : FileStoreIoBase
    {
        private const string ResScheme = "res:";

        public IosFileStore(string basePath) : base(basePath)
        {
        }

        protected override string AppendPath(string path)
        {
            if(path.StartsWith(ResScheme, StringComparison.OrdinalIgnoreCase))
            {
                return path[ResScheme.Length..];
            }

            return base.AppendPath(path);
        }
    }
}