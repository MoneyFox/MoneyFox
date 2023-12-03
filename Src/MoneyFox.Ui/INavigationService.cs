namespace MoneyFox.Ui;

using CommunityToolkit.Mvvm.ComponentModel;

public interface INavigationService
{
    Task GoTo<TViewModel>(object? parameter = null, bool modalNavigation = false) where TViewModel : NavigableViewModel;

    Task GoBack(object? parameter = null);

    Task NavigateToAsync<TPage>() where TPage : ContentPage;
    
    Task OpenModalAsync<T>() where T : ContentPage;

    Task GoBackFromModalAsync();
}

public class NavigableViewModel : ObservableObject
{
    public virtual Task OnNavigatedAsync(object? parameter)
    {
        OnNavigated(parameter);

        return Task.CompletedTask;
    }

    protected virtual void OnNavigated(object? parameter) { }

    public virtual Task OnNavigatedBackAsync(object? parameter)
    {
        OnNavigatedBack(parameter);

        return Task.CompletedTask;
    }

    protected virtual void OnNavigatedBack(object? parameter) { }
}
