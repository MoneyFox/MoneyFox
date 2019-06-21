using MoneyFox.BusinessLogic.FileStore;

namespace MoneyFox.iOS
{
    public class IosFileStore : FileStoreIoBase
    {
        public IosFileStore(bool appendDefaultPath, string basePath) 
            : base(appendDefaultPath, basePath)
        {
        }

        public const string ResScheme = "res:";

        protected override string AppendPath(string path)
        {
            if (path.StartsWith(ResScheme))
                return path.Substring(ResScheme.Length);

            return base.AppendPath(path);
        }
    }
}