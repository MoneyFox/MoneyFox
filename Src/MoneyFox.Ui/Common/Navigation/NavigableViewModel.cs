namespace MoneyFox.Ui.Common.Navigation;

using CommunityToolkit.Mvvm.ComponentModel;

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
