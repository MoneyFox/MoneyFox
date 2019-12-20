namespace MoneyFox.Application.Common.Interfaces
{
    public interface IThemeSelectorAdapter
    {
        string Theme { get; }

        void SetTheme(string theme);
    }
}
