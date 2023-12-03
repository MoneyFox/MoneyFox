namespace MoneyFox.Ui.Common.Services;

using Extensions;
using JetBrains.Annotations;

[UsedImplicitly]
internal sealed class NavigationService(IViewLocator locator, Lazy<INavigation> lazyNavigation) : INavigationService
{
    private INavigation Navigation => lazyNavigation.Value;

    public async Task GoTo<TViewModel>(object? parameter = null) where TViewModel : NavigableViewModel
    {
        var view = locator.GetViewFor<TViewModel>();
        await Navigation.PushAsync((Page)view);

        ((NavigableViewModel)view.BindingContext).OnNavigated(parameter);
    }

    public async Task GoBack(object? parameter = null)
    {
        var view = await Navigation.PopAsync();
        ((NavigableViewModel)view.BindingContext).OnNavigated(parameter);
    }

    public async Task NavigateToAsync<T>() where T : ContentPage
    {
        var pageName = typeof(T).Name;
        await Shell.Current.GoToAsync(pageName);
    }

    public async Task NavigateBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public async Task NavigateBackAsync(string parameterName, string queryParameter)
    {
        await Shell.Current.GoToAsync($"..?{parameterName}={queryParameter}");
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
}

public interface IViewLocator
{
    IBindablePage GetViewFor<TViewModel>() where TViewModel : NavigableViewModel;

    IBindablePage GetView<TView>() where TView : class, IBindablePage;

    Type GetViewTypeFor<TViewModel>() where TViewModel : NavigableViewModel;
}

public interface IBindablePage
{
    object BindingContext { get; set; }
}

internal class ViewLocator : IViewLocator
{
    public IBindablePage GetViewFor<TViewModel>() where TViewModel : NavigableViewModel
    {
        throw new NotImplementedException();
    }

    public IBindablePage GetView<TView>() where TView : class, IBindablePage
    {
        throw new NotImplementedException();
    }

    public Type GetViewTypeFor<TViewModel>() where TViewModel : NavigableViewModel
    {
        throw new NotImplementedException();
    }
}
