// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormsNavigationService.cs" company="The Silly Company">
//   The Silly Company 2016. All rights reserved.
// </copyright>
// <summary>
//   The forms navigation service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MoneyFox.Ui.Navigation.Impl
{

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Views;

    /// <summary>
    /// The forms navigation service.
    /// </summary>
    public class FormsNavigationService : INavigationService
    {
        /// <summary>
        /// The lazy forms navigation.
        /// </summary>
        private readonly Lazy<NavigationPage> lazyFormsNavigation;

        /// <summary>
        /// The view locator.
        /// </summary>
        private readonly IViewLocator viewLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsNavigationService"/> class.
        /// </summary>
        /// <param name="lazyFormsNavigation">
        /// The lazy forms navigation.
        /// </param>
        /// <param name="viewLocator">
        /// The view locator.
        /// </param>
        public FormsNavigationService(Lazy<NavigationPage> lazyFormsNavigation, IViewLocator viewLocator)
        {
            this.lazyFormsNavigation = lazyFormsNavigation;
            this.viewLocator = viewLocator;
        }

        /// <summary>
        /// The navigation page.
        /// </summary>
        private NavigationPage NavigationPage => lazyFormsNavigation.Value;

        /// <summary>
        /// The forms navigation.
        /// </summary>
        private INavigation FormsNavigation => lazyFormsNavigation.Value.Navigation;

        public async Task NavigateToViewModelAsync<TViewModel>(
            object parameter = null,
            bool modalNavigation = false,
            bool clearStack = false,
            bool animated = true)
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
                    FormsNavigation.InsertPageBefore(newRootView, rootPage);
                    rootPage = newRootView;
                }

                // Then we want to go back to root page and clear the stack
                await NavigationPage.PopToRootAsync(animated);
                ((BasePageViewModel)rootPage.BindingContext).OnNavigated(parameter);
                return;
            }

            var view = viewLocator.GetViewFor<TViewModel>();

            if (modalNavigation)
            {
                await FormsNavigation.PushModalAsync((Page)view, animated);
            }
            else
            {
                await NavigationPage.PushAsync((Page)view, animated);
            }

            ((BasePageViewModel)view.BindingContext).OnNavigated(parameter);
        }

        public async Task NavigateToViewAsync<TView>(
            object parameter = null,
            bool modalNavigation = false,
            bool clearStack = false,
            bool animated = true)
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
                    FormsNavigation.InsertPageBefore(newRootView, rootPage);
                    rootPage = newRootView;
                }

                // Then we want to go back to root page and clear the stack
                await NavigationPage.PopToRootAsync(animated);
                ((BasePageViewModel)rootPage.BindingContext).OnNavigated(parameter);
                return;
            }

            var view = viewLocator.GetView<TView>();

            if (modalNavigation)
            {
                await FormsNavigation.PushModalAsync((Page)view, animated);
            }
            else
            {
                await NavigationPage.PushAsync((Page)view, animated);
            }

            ((BasePageViewModel)view.BindingContext).OnNavigated(parameter);
        }

        public async Task NavigateFromMenuToAsync<TViewModel>()
            where TViewModel : BasePageViewModel
        {
            var view = viewLocator.GetViewFor<TViewModel>();
            await NavigationPage.PushAsync((Page)view);
            ((BasePageViewModel)view.BindingContext).OnNavigated(null);

            foreach (
                var page in
                FormsNavigation.NavigationStack
                    .Take(FormsNavigation.NavigationStack.Count - 1)
                    .Skip(1))
            {
                FormsNavigation.RemovePage(page);
            }
        }

        public async Task<IBindablePage> NavigateBackAsync(object parameter = null)
        {
            var page = (IBindablePage)await NavigationPage.PopAsync();
            return page;
        }
    }
}
