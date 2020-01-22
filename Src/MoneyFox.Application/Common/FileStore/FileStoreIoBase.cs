using System;
using System.IO;
using NLog;

namespace MoneyFox.Application.Common.FileStore
{
    public class FileStoreIoBase : FileStoreBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public FileStoreIoBase(string basePath)
        {
            BasePath = basePath;
        }

        protected string BasePath { get; }

        public override Stream OpenRead(string path)
        {
            string fullPath = AppendPath(path);

            if (!File.Exists(fullPath)) throw new FileNotFoundException("File could not be opened.", path);

            return File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public override bool TryMove(string from, string destination, bool overwrite)
        {
            try
            {
                string fullFrom = AppendPath(from);
                string fullTo = AppendPath(destination);

                if (!File.Exists(fullFrom))
                {
                    logger.Error("Error during file move {0} : {1}. File does not exist!", from, destination);

                    return false;
                }

                if (File.Exists(fullTo))
                {
                    if (overwrite)
                    {
                        File.Delete(fullTo);
                    }
                    else
                    {
                        return false;
                    }
                }

                File.Move(fullFrom, fullTo);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString);
                return false;
            }
        }

        protected override void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            string fullPath = AppendPath(path);
            if (File.Exists(fullPath)) File.Delete(fullPath);

            using (FileStream fileStream = File.OpenWrite(fullPath))
            {
                streamAction?.Invoke(fileStream);
            }
        }

        protected virtual string AppendPath(string path)
        {
            return Path.Combine(BasePath, path);
        }
    }
}
