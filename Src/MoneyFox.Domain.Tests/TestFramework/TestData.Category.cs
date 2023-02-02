namespace MoneyFox.Domain.Tests.TestFramework;

internal static partial class TestData
{
    public sealed class DefaultCategory : ICategory
    {
        public int Id { get; set; }
        public string Name => "Beverages";
        public string Note => "Water, Beer, Wine and those..";
        public bool RequireNote => false;
    }

    public interface ICategory
    {
        int Id { get; set; }
        string Name { get; }
        string? Note { get; }
        bool RequireNote { get; }
    }
}
