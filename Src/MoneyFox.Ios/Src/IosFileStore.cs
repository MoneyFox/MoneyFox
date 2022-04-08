namespace MoneyFox.iOS
{
    using System;

    public class IosFileStore : FileStoreIoBase
    {
        public IosFileStore(string basePath) : base(basePath)
        {
        }

        private const string ResScheme = "res:";

        protected override string AppendPath(string path)
        {
            if(path.StartsWith(ResScheme, StringComparison.OrdinalIgnoreCase))
            {
                return path.Substring(ResScheme.Length, path.Length - ResScheme.Length);
            }

            return base.AppendPath(path);
        }
    }
}