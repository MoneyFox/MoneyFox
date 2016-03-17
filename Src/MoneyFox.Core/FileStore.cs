using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.ApplicationInsights;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Core
{
    public class FileStore : IFileStore
    {
        public async Task<bool> Exists(string path)
        {
            try
            {
                // this implementation is based on code from https://github.com/MvvmCross/MvvmCross/issues/521
                path = ToFullPath(path);
                var fileName = Path.GetFileName(path);
                var directoryPath = Path.GetDirectoryName(path);
                if (!await FolderExists(directoryPath))
                {
                    return false;
                }

                var directory = await StorageFolder.GetFolderFromPathAsync(directoryPath);
                await directory.GetFileAsync(fileName);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                new TelemetryClient().TrackException(ex);
                return false;
            }
        }

        public async Task<bool> FolderExists(string folderPath)
        {
            try
            {
                folderPath = ToFullPath(folderPath);
                folderPath = folderPath.TrimEnd('\\');

                var thisFolder = await StorageFolder.GetFolderFromPathAsync(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                new TelemetryClient().TrackException(ex);
                return false;
            }
        }

        public async Task<Stream> OpenRead(string path)
        {
            try
            {
                var storageFile = await StorageFileFromRelativePathAsync(path);
                var streamWithContentType = await storageFile.OpenReadAsync();
                return streamWithContentType.AsStreamForRead();
            }
            catch (Exception ex)
            {
                new TelemetryClient().TrackException(ex);
                return null;
            }
        }

        public void DeleteFile(string path)
        {
            SafeDeleteFile(path);
        }

        public async void WriteFile(string path, IEnumerable<byte> contents)
        {
            try
            {
                var storageFile = CreateStorageFileFromRelativePathAsync(path).GetAwaiter().GetResult();
                using (var streamWithContentType = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (var stream = streamWithContentType.AsStreamForWrite())
                    {
                        using (var binaryWriter = new BinaryWriter(stream))
                        {
                            binaryWriter.Write(contents.ToArray());
                            binaryWriter.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new TelemetryClient().TrackException(ex);
            }
        }

        private static async Task<StorageFile> StorageFileFromRelativePathAsync(string path)
        {
            var fullPath = ToFullPath(path);
            var storageFile = await StorageFile.GetFileFromPathAsync(fullPath).AsTask().ConfigureAwait(false);
            return storageFile;
        }

        private static async Task<StorageFile> CreateStorageFileFromRelativePathAsync(string path)
        {
            var fullPath = ToFullPath(path);
            var directory = Path.GetDirectoryName(fullPath);
            var fileName = Path.GetFileName(fullPath);
            var storageFolder = await StorageFolder.GetFolderFromPathAsync(directory).AsTask().ConfigureAwait(false);
            var storageFile =
                await
                    storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting)
                        .AsTask()
                        .ConfigureAwait(false);
            return storageFile;
        }

        private static string ToFullPath(string path)
        {
            var localFolderPath = ApplicationData.Current.LocalFolder.Path;
            return Path.Combine(localFolderPath, path);
        }

        private static async void SafeDeleteFile(string path)
        {
            try
            {
                var toFile = await StorageFileFromRelativePathAsync(path);
                await toFile.DeleteAsync();
            }
            catch (Exception ex)
            {
                new TelemetryClient().TrackException(ex);
            }
        }
    }
}