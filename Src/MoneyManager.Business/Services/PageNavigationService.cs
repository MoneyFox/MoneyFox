using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Views;

namespace MoneyManager.Business.Services
{
    public class PageNavigationService : NavigationService
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();

        public override void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }

                var frame = ((Frame)Window.Current.Content);
                frame.Navigate(_pagesByKey[pageKey], parameter);
            }
        }


        /// <summary>
        /// Adds a key/page pair to the navigation service.
        /// </summary>
        /// <param name="key">The key that will be used later
        /// in the <see cref="NavigateTo(string)"/> or <see cref="NavigateTo(string, object)"/> methods.</param>
        /// <param name="pageType">The type of the page corresponding to the key.</param>
        public new void  Configure(string key, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                {
                    throw new ArgumentException("This key is already used: " + key);
                }

                if (_pagesByKey.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(
                        "This type is already configured with key " + _pagesByKey.First(p => p.Value == pageType).Key);
                }

                _pagesByKey.Add(
                    key,
                    pageType);
            }
        }
    }
}
