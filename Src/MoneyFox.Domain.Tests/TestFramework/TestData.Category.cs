namespace MoneyFox.Core.Tests.TestFramework;

internal static partial class TestData
{
    public sealed class DefaultCategory : ICategory
    {
        public string Name => "Beverages";
        public string Note => "Water, Beer, Wine and those..";
        public bool RequireNote => false;
    }

    public interface ICategory
    {
        string Name { get; }
        string? Note { get; }
        bool RequireNote { get; }
    }
}
