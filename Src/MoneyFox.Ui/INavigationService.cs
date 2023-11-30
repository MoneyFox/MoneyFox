namespace MoneyFox.Ui;

public interface INavigationService
{
    Task GoTo<TViewModel>(object? parameter = null) where TViewModel : NavigableViewModel;

    Task GoBack();

    Task NavigateToAsync<TPage>() where TPage : ContentPage;

    Task NavigateBackAsync();

    Task NavigateBackAsync(string parameterName, string queryParameter);

    Task OpenModalAsync<T>() where T : ContentPage;

    Task GoBackFromModalAsync();
}

public class NavigableViewModel
{
    public virtual void OnNavigated(object parameter)
    {
    }
}
