namespace MoneyFox.Ui.Platforms.MacCatalyst.Src;

using Common;

public class IosFileStore : FileStoreIoBase
{
    private const string RES_SCHEME = "res:";

    public IosFileStore(string basePath) : base(basePath) { }

    protected override string AppendPath(string path)
    {
        if (path.StartsWith(value: RES_SCHEME, comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            return path.Substring(startIndex: RES_SCHEME.Length, length: path.Length - RES_SCHEME.Length);
        }

        return base.AppendPath(path);
    }
}
