namespace MoneyFox.Presentation.Interfaces
{
    public interface IThemeSelectorAdapter
    {
        string Theme { get; }

        void SetTheme(string theme);
    }
}
