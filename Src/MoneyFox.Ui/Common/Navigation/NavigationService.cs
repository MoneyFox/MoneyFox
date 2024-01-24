namespace MoneyFox.Ui.Common.Navigation;

using Aptabase.Maui;
using JetBrains.Annotations;

[UsedImplicitly]
internal sealed class NavigationService(Lazy<NavigationPage> lazyNavigation, IViewLocator locator, IAptabaseClient aptabaseClient) : INavigationService
{
    private NavigationPage NavigationPage => lazyNavigation.Value;
    private INavigation Navigation => NavigationPage.Navigation;

    public async Task GoBack(object? parameter = null)
    {
        await NavigationPage.PopAsync();
    }

    public async Task NavigateFromMenuToAsync<TViewModel>() where TViewModel : NavigableViewModel
    {
        var view = locator.GetViewFor<TViewModel>();
        await NavigationPage.PushAsync((Page)view);
        await ((NavigableViewModel)view.BindingContext).OnNavigatedAsync(null);
        foreach (var page in Navigation.NavigationStack.Take(Navigation.NavigationStack.Count - 1).Skip(1))
        {
            Navigation.RemovePage(page);
        }
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

        aptabaseClient.TrackEvent($"Navigate to {typeof(TViewModel).Name}");
        await ((NavigableViewModel)view.BindingContext).OnNavigatedAsync(parameter);
    }
}
