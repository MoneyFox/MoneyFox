using MoneyFox.Presentation;

namespace MoneyFox.iOS
{
    public class IosFileStore : FileStoreIoBase
    {
        public IosFileStore(string basePath) : base(basePath)
        { }

        public const string ResScheme = "res:";

        protected override string AppendPath(string path)
        {
            if (path.StartsWith(ResScheme))
                return path.Substring(ResScheme.Length);

            return base.AppendPath(path);
        }
    }
}
