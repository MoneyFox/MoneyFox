namespace MoneyFox.Ui.Platforms.iOS.Src;

using Common;

public class IosFileStore : FileStoreIoBase
{
    private const string ResScheme = "res:";

    public IosFileStore(string basePath) : base(basePath) { }

    protected override string AppendPath(string path)
    {
        if (path.StartsWith(value: ResScheme, comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            return path.Substring(startIndex: ResScheme.Length, length: path.Length - ResScheme.Length);
        }

        return base.AppendPath(path);
    }
}
