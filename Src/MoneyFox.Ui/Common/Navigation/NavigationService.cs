namespace MoneyFox.Ui.Common.Navigation;

using Aptabase.Maui;
using JetBrains.Annotations;

[UsedImplicitly]
internal sealed class NavigationService(IViewLocator locator, IAptabaseClient aptabaseClient, Lazy<NavigationPage>? lazyFormsNavigation = null) : INavigationService
{
    private INavigation? MauiNavigation => (Application.Current?.MainPage as NavigationPage)?.Navigation;

    private INavigation Navigation =>  MauiNavigation ?? Shell.Current.Navigation;

    public async Task GoBack(object? parameter = null)
    {
        await Navigation.PopAsync();
        var view = Shell.Current.CurrentPage;
        await ((NavigableViewModel)view.BindingContext).OnNavigatedBackAsync(parameter);
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

        aptabaseClient.TrackEvent($"Navigate to {nameof(TViewModel)}");
        await ((NavigableViewModel)view.BindingContext).OnNavigatedAsync(parameter);
    }
}
