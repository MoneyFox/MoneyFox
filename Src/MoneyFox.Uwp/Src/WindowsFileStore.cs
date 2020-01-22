using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using MoneyFox.Application.Common.FileStore;
using NLog;

namespace MoneyFox.Uwp.Src
{
    public class WindowsFileStore : FileStoreBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override Stream OpenRead(string path)
        {
            try
            {
                StorageFile storageFile = StorageFileFromRelativePath(path);
                IRandomAccessStreamWithContentType streamWithContentType = storageFile.OpenReadAsync().Await();

                return streamWithContentType.AsStreamForRead();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        public override bool TryMove(string from, string destination, bool overwrite)
        {
            try
            {
                StorageFile fromFile = StorageFileFromRelativePath(from);

                if (overwrite && !SafeDeleteFile(destination)) return false;

                string fullToPath = ToFullPath(destination);
                string toDirectory = Path.GetDirectoryName(fullToPath);
                string toFileName = Path.GetFileName(fullToPath);
                StorageFolder toStorageFolder = StorageFolder.GetFolderFromPathAsync(toDirectory).Await();
                fromFile.MoveAsync(toStorageFolder, toFileName).Await();

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        protected override void WriteFileCommon(string path, Action<Stream> streamAction)
        {
            SafeDeleteFile(path);

            try
            {
                StorageFile storageFile = CreateStorageFileFromRelativePathAsync(path).GetAwaiter().GetResult();
                using (IRandomAccessStream streamWithContentType = storageFile.OpenAsync(FileAccessMode.ReadWrite).Await())
                {
                    using (Stream stream = streamWithContentType.AsStreamForWrite())
                    {
                        streamAction(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        private static StorageFile StorageFileFromRelativePath(string path)
        {
            string fullPath = ToFullPath(path);
            StorageFile storageFile = StorageFile.GetFileFromPathAsync(fullPath).Await();

            return storageFile;
        }

        private static bool SafeDeleteFile(string path)
        {
            try
            {
                StorageFile toFile = StorageFileFromRelativePath(path);
                toFile.DeleteAsync().Await();

                return true;
            }
            catch (FileNotFoundException)
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<StorageFile> CreateStorageFileFromRelativePathAsync(string path)
        {
            string fullPath = ToFullPath(path);
            string directory = Path.GetDirectoryName(fullPath);
            string fileName = Path.GetFileName(fullPath);
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(directory).AsTask().ConfigureAwait(false);
            StorageFile storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting)
                                                         .AsTask()
                                                         .ConfigureAwait(false);

            return storageFile;
        }

        private static string ToFullPath(string path)
        {
            string localFolderPath = ApplicationData.Current.LocalFolder.Path;

            return Path.Combine(localFolderPath, path);
        }
    }
}
