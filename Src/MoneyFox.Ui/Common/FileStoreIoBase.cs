namespace MoneyFox.Ui.Common;

using Infrastructure;
using Serilog;

public class FileStoreIoBase : FileStoreBase
{
    public FileStoreIoBase(string basePath)
    {
        BasePath = basePath;
    }

    protected string BasePath { get; }

    public override async Task<Stream> OpenReadAsync(string path)
    {
        var fullPath = AppendPath(path);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException(message: "File could not be opened.", fileName: path);
        }

        return await Task.FromResult(File.Open(path: fullPath, mode: FileMode.Open, access: FileAccess.Read, share: FileShare.ReadWrite));
    }

    public override async Task<bool> TryMoveAsync(string from, string destination, bool overwrite)
    {
        try
        {
            var fullFrom = AppendPath(from);
            var fullTo = AppendPath(destination);
            if (!File.Exists(fullFrom))
            {
                Log.Error(
                    messageTemplate: "Error during file move {from} : {destination}. File does not exist!",
                    propertyValue0: from,
                    propertyValue1: destination);

                return await Task.FromResult(false);
            }

            if (File.Exists(fullTo))
            {
                if (overwrite)
                {
                    File.Delete(fullTo);
                }
                else
                {
                    return await Task.FromResult(false);
                }
            }

            File.Move(sourceFileName: fullFrom, destFileName: fullTo);

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            Log.Error(exception: ex, messageTemplate: "Error during moving file");

            return await Task.FromResult(false);
        }
    }

    protected override Task WriteFileCommonAsync(string path, Action<Stream> streamAction)
    {
        var fullPath = AppendPath(path);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        using (var fileStream = File.OpenWrite(fullPath))
        {
            streamAction.Invoke(fileStream);
        }

        return Task.CompletedTask;
    }

    protected virtual string AppendPath(string path)
    {
        return Path.Combine(path1: BasePath, path2: path);
    }
}
