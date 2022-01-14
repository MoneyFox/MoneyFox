using MoneyFox.Application.Common.FileStore;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MoneyFox
{
    public class FileStoreIoBase : FileStoreBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public FileStoreIoBase(string basePath)
        {
            BasePath = basePath;
        }

        protected string BasePath { get; }

        public override async Task<Stream> OpenReadAsync(string path)
        {
            string fullPath = AppendPath(path);

            if(!File.Exists(fullPath))
            {
                throw new FileNotFoundException("File could not be opened.", path);
            }

            return await Task.FromResult(File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public override async Task<bool> TryMoveAsync(string from, string destination, bool overwrite)
        {
            try
            {
                string fullFrom = AppendPath(from);
                string fullTo = AppendPath(destination);

                if(!File.Exists(fullFrom))
                {
                    logger.Error("Error during file move {0} : {1}. File does not exist!", from, destination);

                    return await Task.FromResult(false);
                }

                if(File.Exists(fullTo))
                {
                    if(overwrite)
                    {
                        File.Delete(fullTo);
                    }
                    else
                    {
                        return await Task.FromResult(false);
                    }
                }

                File.Move(fullFrom, fullTo);

                return await Task.FromResult(true);
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString);
                return await Task.FromResult(false);
            }
        }

        protected override Task WriteFileCommonAsync(string path, Action<Stream> streamAction)
        {
            string fullPath = AppendPath(path);
            if(File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            using(FileStream fileStream = File.OpenWrite(fullPath))
            {
                streamAction?.Invoke(fileStream);
            }

            return Task.CompletedTask;
        }

        protected virtual string AppendPath(string path) => Path.Combine(BasePath, path);
    }
}