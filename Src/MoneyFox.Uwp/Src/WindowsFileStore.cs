using MoneyFox.Application.Common.FileStore;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace MoneyFox.Uwp.Src
{
    public class WindowsFileStore : FileStoreBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override async Task<Stream> OpenReadAsync(string path)
        {
            try
            {
                StorageFile storageFile = await StorageFileFromRelativePathAsync(path);
                IRandomAccessStreamWithContentType streamWithContentType = await storageFile.OpenReadAsync();

                return streamWithContentType.AsStreamForRead();
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        public override async Task<bool> TryMoveAsync(string from, string destination, bool overwrite)
        {
            try
            {
                StorageFile fromFile = await StorageFileFromRelativePathAsync(from);

                if(overwrite && !await SafeDeleteFile(destination))
                {
                    return false;
                }

                string fullToPath = ToFullPath(destination);
                string toDirectory = Path.GetDirectoryName(fullToPath);
                string toFileName = Path.GetFileName(fullToPath);
                StorageFolder toStorageFolder = await StorageFolder.GetFolderFromPathAsync(toDirectory);
                await fromFile.MoveAsync(toStorageFolder, toFileName);

                return true;
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        protected override async Task WriteFileCommonAsync(string path, Action<Stream> streamAction)
        {
            await SafeDeleteFile(path);

            try
            {
                StorageFile storageFile = await CreateStorageFileFromRelativePathAsync(path);
                using(IRandomAccessStream streamWithContentType = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using(Stream stream = streamWithContentType.AsStreamForWrite())
                    {
                        streamAction(stream);
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        private static async Task<StorageFile> StorageFileFromRelativePathAsync(string path)
        {
            string fullPath = ToFullPath(path);
            StorageFile storageFile = await StorageFile.GetFileFromPathAsync(fullPath);

            return storageFile;
        }

        private static async Task<bool> SafeDeleteFile(string path)
        {
            try
            {
                StorageFile toFile = await StorageFileFromRelativePathAsync(path);
                await toFile.DeleteAsync();

                return true;
            }
            catch(FileNotFoundException)
            {
                return true;
            }
            catch(Exception)
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
