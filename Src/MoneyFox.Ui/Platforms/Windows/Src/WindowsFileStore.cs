namespace MoneyFox.Ui.Platforms.Windows.Src;

using global::Windows.Storage;
using Infrastructure;
using Serilog;

public class WindowsFileStore : FileStoreBase
{
    public override async Task<Stream> OpenReadAsync(string path)
    {
        try
        {
            var storageFile = await StorageFileFromRelativePathAsync(path);
            var streamWithContentType = await storageFile.OpenReadAsync();

            return streamWithContentType.AsStreamForRead();
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Failed during read stream");

            return null;
        }
    }

    public override async Task<bool> TryMoveAsync(string from, string destination, bool overwrite)
    {
        try
        {
            var fromFile = await StorageFileFromRelativePathAsync(from);
            if (overwrite && !await SafeDeleteFileAsync(destination))
            {
                return false;
            }

            var fullToPath = ToFullPath(destination);
            var toDirectory = Path.GetDirectoryName(fullToPath);
            var toFileName = Path.GetFileName(fullToPath);
            var toStorageFolder = await StorageFolder.GetFolderFromPathAsync(toDirectory);
            await fromFile.MoveAsync(destinationFolder: toStorageFolder, desiredNewName: toFileName);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Failed to move file");

            return false;
        }
    }

    protected override async Task WriteFileCommonAsync(string path, Action<Stream> streamAction)
    {
        await SafeDeleteFileAsync(path);
        try
        {
            var storageFile = await CreateStorageFileFromRelativePathAsync(path);
            using var streamWithContentType = await storageFile.OpenAsync(FileAccessMode.ReadWrite);
            using var stream = streamWithContentType.AsStreamForWrite();
            streamAction(stream);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Failed to write file");

            throw;
        }
    }

    private static async Task<StorageFile> StorageFileFromRelativePathAsync(string path)
    {
        var fullPath = ToFullPath(path);
        var storageFile = await StorageFile.GetFileFromPathAsync(fullPath);

        return storageFile;
    }

    private static async Task<bool> SafeDeleteFileAsync(string path)
    {
        try
        {
            var toFile = await StorageFileFromRelativePathAsync(path);
            await toFile.DeleteAsync();

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
        var fullPath = ToFullPath(path);
        var directory = Path.GetDirectoryName(fullPath);
        var fileName = Path.GetFileName(fullPath);
        var storageFolder = await StorageFolder.GetFolderFromPathAsync(directory).AsTask().ConfigureAwait(false);
        var storageFile = await storageFolder.CreateFileAsync(desiredName: fileName, options: CreationCollisionOption.ReplaceExisting)
            .AsTask()
            .ConfigureAwait(false);

        return storageFile;
    }

    private static string ToFullPath(string path)
    {
        var localFolderPath = ApplicationData.Current.LocalFolder.Path;

        return Path.Combine(path1: localFolderPath, path2: path);
    }
}
