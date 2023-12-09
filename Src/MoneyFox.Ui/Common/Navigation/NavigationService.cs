namespace MoneyFox.Ui.Common.Navigation;

using Extensions;
using JetBrains.Annotations;

[UsedImplicitly]
internal sealed class NavigationService(IViewLocator locator) : INavigationService
{
    private INavigation Navigation => Shell.Current.Navigation;

    public async Task GoBack(object? parameter = null)
    {
        await Navigation.PopAsync();
        var view = Shell.Current.CurrentPage;
        await ((NavigableViewModel)view.BindingContext).OnNavigatedBackAsync(parameter);
    }

    public async Task OpenModalAsync<T>() where T : ContentPage
    {
        var pageName = typeof(T).Name;
        await Shell.Current.GoToModalAsync(pageName);
    }

    public async Task GoBackFromModalAsync()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }

    public async Task GoTo<TViewModel>(object? parameter = null, bool modalNavigation = false) where TViewModel : NavigableViewModel
    {
        var view = locator.GetViewFor<TViewModel>();
        if (modalNavigation)
        {
            await Navigation.PushModalAsync((Page)view);
        }
        else
        {
            await Navigation.PushAsync((Page)view);
        }

        await ((NavigableViewModel)view.BindingContext).OnNavigatedAsync(parameter);
    }
}
