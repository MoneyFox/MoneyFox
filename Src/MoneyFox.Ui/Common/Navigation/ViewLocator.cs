namespace MoneyFox.Ui.Common.Navigation;

using Extensions;
using Views.Accounts.AccountList;
using Views.Dashboard;

internal class ViewLocator(IServiceProvider serviceProvider) : IViewLocator
{
    private static readonly List<(Type ViewModelType, Type ViewType)> ViewLocatorDictionary = new()
    {
        (typeof(DashboardViewModel), typeof(DashboardPage)), (typeof(AccountListViewModel), typeof(AccountListPage))
    };

    private static readonly List<(Type ViewModelType, Type ViewType)> DesktopViewLocatorDictionary = new()
    {
        (typeof(DashboardViewModel), typeof(DashboardPage)), (typeof(AccountListViewModel), typeof(DesktopAccountListPage))
    };

    public IBindablePage GetViewFor<TViewModel>() where TViewModel : NavigableViewModel
    {
        var viewModel = serviceProvider.GetService<TViewModel>();
        var view = (IBindablePage)serviceProvider.GetService(FindViewByViewModel(typeof(TViewModel)));
        view.BindingContext = viewModel;

        return view;
    }

    public IBindablePage GetView<TView>() where TView : class, IBindablePage
    {
        var view = (IBindablePage)serviceProvider.GetService(typeof(TView));
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
        var viewLocatorDictionary = DeviceInfo.Current.Idiom.UseDesktopPage() ? DesktopViewLocatorDictionary : ViewLocatorDictionary;
        foreach (var pair in viewLocatorDictionary)
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
        var viewLocatorDictionary = DeviceInfo.Current.Idiom.UseDesktopPage() ? DesktopViewLocatorDictionary : ViewLocatorDictionary;
        foreach (var pair in viewLocatorDictionary)
        {
            if (pair.ViewModelType == viewModelType)
            {
                return pair.ViewType;
            }
        }

        return null;
    }
}
