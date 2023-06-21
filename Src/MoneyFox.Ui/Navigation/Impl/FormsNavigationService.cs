// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormsNavigationService.cs" company="The Silly Company">
//   The Silly Company 2016. All rights reserved.
// </copyright>
// <summary>
//   The forms navigation service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MoneyFox.Ui.Navigation.Impl;

using Views;

public class FormsNavigationService : INavigationService
{
    private readonly Lazy<NavigationPage> lazyFormsNavigation;

    private readonly IViewLocator viewLocator;

    public FormsNavigationService(Lazy<NavigationPage> lazyFormsNavigation, IViewLocator viewLocator)
    {
        this.lazyFormsNavigation = lazyFormsNavigation;
        this.viewLocator = viewLocator;
    }

    private NavigationPage NavigationPage => lazyFormsNavigation.Value;

    private INavigation FormsNavigation => lazyFormsNavigation.Value.Navigation;

    public async Task NavigateToViewModelAsync<TViewModel>(object? parameter = null, bool modalNavigation = false, bool clearStack = false, bool animated = true)
        where TViewModel : BasePageViewModel
    {
        if (clearStack)
        {
            var viewType = viewLocator.GetViewTypeFor<TViewModel>();
            var rootPage = FormsNavigation.NavigationStack.First();
            if (viewType != rootPage.GetType())
            {
                var newRootView = (Page)viewLocator.GetViewFor<TViewModel>();

                // Make the new view the root of our navigation stack
                FormsNavigation.InsertPageBefore(page: newRootView, before: rootPage);
                rootPage = newRootView;
            }

            // Then we want to go back to root page and clear the stack
            await NavigationPage.PopToRootAsync(animated);
            await ((BasePageViewModel)rootPage.BindingContext).OnNavigatedAsync(parameter);

            return;
        }

        var view = viewLocator.GetViewFor<TViewModel>();
        if (modalNavigation)
        {
            await FormsNavigation.PushModalAsync(page: (Page)view, animated: animated);
        }
        else
        {
            await NavigationPage.PushAsync(page: (Page)view, animated: animated);
        }

        await ((BasePageViewModel)view.BindingContext).OnNavigatedAsync(parameter);
    }

    public async Task NavigateToViewAsync<TView>(object? parameter = null, bool modalNavigation = false, bool clearStack = false, bool animated = true)
        where TView : class, IBindablePage
    {
        if (clearStack)
        {
            var viewType = typeof(TView);
            var rootPage = FormsNavigation.NavigationStack.First();
            if (viewType != rootPage.GetType())
            {
                var newRootView = (Page)viewLocator.GetView<TView>();

                // Make the new view the root of our navigation stack
                FormsNavigation.InsertPageBefore(page: newRootView, before: rootPage);
                rootPage = newRootView;
            }

            // Then we want to go back to root page and clear the stack
            await NavigationPage.PopToRootAsync(animated);
            await ((BasePageViewModel)rootPage.BindingContext).OnNavigatedAsync(parameter);

            return;
        }

        var view = viewLocator.GetView<TView>();
        if (modalNavigation)
        {
            await FormsNavigation.PushModalAsync(page: (Page)view, animated: animated);
        }
        else
        {
            await NavigationPage.PushAsync(page: (Page)view, animated: animated);
        }

        await ((BasePageViewModel)view.BindingContext).OnNavigatedAsync(parameter);
    }

    public async Task NavigateFromMenuToAsync<TViewModel>() where TViewModel : BasePageViewModel
    {
        var view = viewLocator.GetViewFor<TViewModel>();
        await NavigationPage.PushAsync((Page)view);
        await ((BasePageViewModel)view.BindingContext).OnNavigatedAsync(null);
        foreach (var page in FormsNavigation.NavigationStack.Take(FormsNavigation.NavigationStack.Count - 1).Skip(1))
        {
            FormsNavigation.RemovePage(page);
        }
    }

    public async Task<IBindablePage> NavigateBackAsync(object? parameter = null)
    {
        var page = (IBindablePage)await NavigationPage.PopAsync();
        await ((BasePageViewModel)page.BindingContext).OnNavigatedBackAsync(parameter);
        return page;
    }
}
