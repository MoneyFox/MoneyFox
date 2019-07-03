using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using MoneyFox.Application.FileStore;
using NLog;

namespace MoneyFox.Uwp.Business
{
    public class WindowsFileStore : FileStoreBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public override Stream OpenRead(string path)
        {
            try
            {
                var storageFile = StorageFileFromRelativePath(path);
                var streamWithContentType = storageFile.OpenReadAsync().Await();
                return streamWithContentType.AsStreamForRead();
            } catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        public override bool TryMove(string @from, string destination, bool overwrite)
        {
            try
            {
                var fromFile = StorageFileFromRelativePath(from);

                if (overwrite && !SafeDeleteFile(destination))
                {
                    return false;
                }

                var fullToPath = ToFullPath(destination);
                var toDirectory = Path.GetDirectoryName(fullToPath);
                var toFileName = Path.GetFileName(fullToPath);
                var toStorageFolder = StorageFolder.GetFolderFromPathAsync(toDirectory).Await();
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
                var storageFile = CreateStorageFileFromRelativePathAsync(path).GetAwaiter().GetResult();
                using (var streamWithContentType = storageFile.OpenAsync(FileAccessMode.ReadWrite).Await())
                {
                    using (var stream = streamWithContentType.AsStreamForWrite())
                    {
                        streamAction(stream);
                    }
                }
            } catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        private static StorageFile StorageFileFromRelativePath(string path)
        {
            var fullPath = ToFullPath(path);
            var storageFile = StorageFile.GetFileFromPathAsync(fullPath).Await();
            return storageFile;
        }

        private static bool SafeDeleteFile(string path)
        {
            try
            {
                var toFile = StorageFileFromRelativePath(path);
                toFile.DeleteAsync().Await();
                return true;
            } catch (FileNotFoundException)
            {
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        private static async Task<StorageFile> CreateStorageFileFromRelativePathAsync(string path)
        {
            var fullPath = ToFullPath(path);
            var directory = Path.GetDirectoryName(fullPath);
            var fileName = Path.GetFileName(fullPath);
            var storageFolder = await StorageFolder.GetFolderFromPathAsync(directory).AsTask().ConfigureAwait(false);
            var storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);
            return storageFile;
        }

        private static string ToFullPath(string path)
        {
            var localFolderPath = ApplicationData.Current.LocalFolder.Path;
            return Path.Combine(localFolderPath, path);
        }
    }
}
