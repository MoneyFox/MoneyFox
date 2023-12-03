namespace MoneyFox.Ui;

using CommunityToolkit.Mvvm.ComponentModel;

public interface INavigationService
{
    Task GoTo<TViewModel>(object? parameter = null) where TViewModel : NavigableViewModel;

    Task GoBack(object? parameter = null);

    Task NavigateToAsync<TPage>() where TPage : ContentPage;

    Task NavigateBackAsync();

    Task NavigateBackAsync(string parameterName, string queryParameter);

    Task OpenModalAsync<T>() where T : ContentPage;

    Task GoBackFromModalAsync();
}

public class NavigableViewModel : ObservableRecipient
{
    public virtual void OnNavigated(object? parameter)
    {
    }
}
