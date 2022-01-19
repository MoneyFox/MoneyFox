namespace MoneyFox.Core._Pending_.Common.Interfaces
{
    public interface IThemeSelectorAdapter
    {
        string Theme { get; }

        void SetTheme(string theme);
    }
}