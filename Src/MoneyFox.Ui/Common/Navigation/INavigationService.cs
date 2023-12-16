namespace MoneyFox.Ui.Common.Navigation;

public interface INavigationService
{
    Task GoTo<TViewModel>(object? parameter = null, bool modalNavigation = false) where TViewModel : NavigableViewModel;

    Task GoBack(object? parameter = null);
}
