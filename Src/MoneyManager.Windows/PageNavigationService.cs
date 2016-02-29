using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Views;

namespace MoneyManager.Windows
{
    public class PageNavigationService : INavigationService
    {
        /// <summary>
        ///     The key that is returned by the <see cref="CurrentPageKey" /> property
        ///     when the current Page is the root page.
        /// </summary>
        public const string ROOT_PAGE_KEY = "-- ROOT --";

        /// <summary>
        ///     The key that is returned by the <see cref="CurrentPageKey" /> property
        ///     when the current Page is not found.
        ///     This can be the case when the navigation wasn't managed by this NavigationService,
        ///     for example when it is directly triggered in the code behind, and the
        ///     NavigationService was not configured for this page type.
        /// </summary>
        public const string UNKNOWN_PAGE_KEY = "-- UNKNOWN --";

        private readonly Dictionary<string, Type> pagesByKey = new Dictionary<string, Type>();

        /// <summary>
        ///     The key corresponding to the currently displayed page.
        /// </summary>
        public string CurrentPageKey
        {
            get
            {
                lock (pagesByKey)
                {
                    var frame = ((AppShell) Window.Current.Content).AppFrame;

                    if (frame.BackStackDepth == 0)
                    {
                        return ROOT_PAGE_KEY;
                    }

                    if (frame.Content == null)
                    {
                        return UNKNOWN_PAGE_KEY;
                    }

                    var currentType = frame.Content.GetType();

                    if (pagesByKey.All(p => p.Value != currentType))
                    {
                        return UNKNOWN_PAGE_KEY;
                    }

                    var item = pagesByKey.FirstOrDefault(
                        i => i.Value == currentType);

                    return item.Key;
                }
            }
        }

        /// <summary>
        ///     If possible, discards the current page and displays the previous page
        ///     on the navigation stack.
        /// </summary>
        public void GoBack()
        {
            var shell = ((AppShell) Window.Current.Content);

            if (shell.AppFrame.CanGoBack)
            {
                shell.AppFrame.GoBack();
            }
        }

        /// <summary>
        ///     Displays a new page corresponding to the given key.
        ///     Make sure to call the <see cref="Configure" />
        ///     method first.
        /// </summary>
        /// <param name="pageKey">
        ///     The key corresponding to the page
        ///     that should be displayed.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     When this method is called for
        ///     a key that has not been configured earlier.
        /// </exception>
        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        /// <summary>
        ///     Displays a new page corresponding to the given key,
        ///     and passes a parameter to the new page.
        ///     Make sure to call the <see cref="Configure" />
        ///     method first.
        /// </summary>
        /// <param name="pageKey">
        ///     The key corresponding to the page
        ///     that should be displayed.
        /// </param>
        /// <param name="parameter">
        ///     The parameter that should be passed
        ///     to the new page.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     When this method is called for
        ///     a key that has not been configured earlier.
        /// </exception>
        public void NavigateTo(string pageKey, object parameter)
        {
            lock (pagesByKey)
            {
                if (!pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }

                var shell = ((AppShell) Window.Current.Content);
                shell.AppFrame.Navigate(pagesByKey[pageKey], parameter);
            }
        }

        /// <summary>
        ///     Adds a key/page pair to the navigation service.
        /// </summary>
        /// <param name="key">
        ///     The key that will be used later
        ///     in the <see cref="NavigateTo(string)" /> or <see cref="NavigateTo(string, object)" /> methods.
        /// </param>
        /// <param name="pageType">The type of the page corresponding to the key.</param>
        public void Configure(string key, Type pageType)
        {
            lock (pagesByKey)
            {
                if (pagesByKey.ContainsKey(key))
                {
                    throw new ArgumentException("This key is already used: " + key);
                }

                if (pagesByKey.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(
                        "This type is already configured with key " + pagesByKey.First(p => p.Value == pageType).Key);
                }

                pagesByKey.Add(
                    key,
                    pageType);
            }
        }
    }
}