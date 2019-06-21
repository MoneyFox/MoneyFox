using System;
using System.IO;
using MvvmCross.Exceptions;
using NLog;

namespace MoneyFox.BusinessLogic.FileStore
{
    public class FileStoreIoBase : FileStoreBase
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public FileStoreIoBase(bool appendDefaultPath, string basePath)
        {
            BasePath = basePath;
            AppendDefaultPath = appendDefaultPath;
        }

        protected string BasePath { get; }

        protected bool AppendDefaultPath { get; }

        public override Stream OpenRead(string path)
        {
            var fullPath = FullPath(path);
            if (!File.Exists(fullPath))
            {
                return null;
            }

            return File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public override bool TryMove(string @from, string to, bool overwrite)
        {
            try
            {
                var fullFrom = FullPath(from);
                var fullTo = FullPath(to);

                if (!System.IO.File.Exists(fullFrom))
                {
                    logger.Error("Error during file move {0} : {1}. File does not exist!", from, to);
                    return false;
                }

                if (System.IO.File.Exists(fullTo))
                {
                    if (overwrite)
                    {
                        System.IO.File.Delete(fullTo);
                    } else
                    {
                        return false;
                    }
                }

                System.IO.File.Move(fullFrom, fullTo);
                return true;
            } 
            catch (Exception ex)
            {
                logger.Error(ex.ToLongString);return false;
            }
        }

        protected override void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            throw new NotImplementedException();
        }

        private string FullPath(string path)
        {
            if (!AppendDefaultPath) return path;

            return AppendPath(path);
        }

        protected virtual string AppendPath(string path)
            => Path.Combine(BasePath, path);
    }
}
