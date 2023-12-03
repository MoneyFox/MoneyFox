namespace MoneyFox.Ui.Common.Services;

using Extensions;
using JetBrains.Annotations;
using Views.Dashboard;

[UsedImplicitly]
internal sealed class NavigationService(IViewLocator locator) : INavigationService
{
    private INavigation Navigation => Shell.Current.Navigation;

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

internal class ViewLocator(IServiceProvider serviceProvider) : IViewLocator
{
    private static readonly List<(Type ViewModelType, Type ViewType)> ViewLocatorDictionary = new List<(Type, Type)>
    {
        (typeof(DashboardViewModel), typeof(DashboardPage)),
    };

    public IBindablePage GetViewFor<TViewModel>() where TViewModel : NavigableViewModel
    {
        var viewModel = serviceProvider.GetService<TViewModel>();
        var view =
            (IBindablePage)serviceProvider.GetService(FindViewByViewModel(typeof(TViewModel)));
        view.BindingContext = viewModel;
        return view;
    }

    public IBindablePage GetView<TView>() where TView : class, IBindablePage
    {
        var view =
            (IBindablePage)serviceProvider.GetService(typeof(TView));
        var viewModel = serviceProvider.GetService(FindViewModelByView(typeof(TView)));
        view.BindingContext = viewModel;
        return view;
    }

    public Type GetViewTypeFor<TViewModel>() where TViewModel : NavigableViewModel
    {
        return FindViewByViewModel(typeof(TViewModel));
    }


    private static Type FindViewModelByView(Type viewType)
    {
        foreach (var pair in ViewLocatorDictionary)
        {
            if (pair.ViewType == viewType)
            {
                return pair.ViewModelType;
            }
        }

        return null;
    }

    private static Type FindViewByViewModel(Type viewModelType)
    {
        foreach (var pair in ViewLocatorDictionary)
        {
            if (pair.ViewModelType == viewModelType)
            {
                return pair.ViewType;
            }
        }

        return null;
    }
}
