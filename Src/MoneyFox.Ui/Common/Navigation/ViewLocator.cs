namespace MoneyFox.Ui.Common.Navigation;

using MoneyFox.Ui.Views.Dashboard;

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
