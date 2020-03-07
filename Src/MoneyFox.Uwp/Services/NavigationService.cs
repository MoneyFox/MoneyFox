using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Services
{
    public class NavigationService : INavigationService
    {
        public event NavigatedEventHandler Navigated;

        public event NavigationFailedEventHandler NavigationFailed;

        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();

        private Frame frame;
        private object lastParamUsed;

        public Frame Frame
        {
            get
            {
                if (frame == null)
                {
                    frame = Window.Current.Content as Frame;
                    RegisterFrameEvents();
                }

                return frame;
            }

            set
            {
                UnregisterFrameEvents();
                frame = value;
                RegisterFrameEvents();
            }
        }

        public bool CanGoBack => Frame.CanGoBack;

        public bool CanGoForward => Frame.CanGoForward;

        public bool GoBack()
        {
            if (CanGoBack)
            {
                Frame.GoBack();
                return true;
            }

            return false;
        }

        public bool GoForward()
        {
            if (CanGoForward)
            {
                Frame.GoForward();
                return true;
            }
            return false;
        }

        public bool Navigate(string pageKey, object parameter = null, NavigationTransitionInfo infoOverride = null)
        {
            Type page;
            lock (_pages)
            {
                if (!_pages.TryGetValue(pageKey, out page))
                {
                    throw new
                        ArgumentException($"Page not found: {pageKey}. Did you forget to call NavigationService.Configure?",
                                          nameof(pageKey));
                }
            }

            if (Frame.Content?.GetType() != page || parameter != null && !parameter.Equals(lastParamUsed))
            {
                bool navigationResult = Frame.Navigate(page, parameter, infoOverride);
                if (navigationResult)
                    lastParamUsed = parameter;

                return navigationResult;
            }

            return false;
        }

        public void Configure(string key, Type pageType)
        {
            lock (_pages)
            {
                if (_pages.ContainsKey(key))
                    throw new ArgumentException(string.Format("The key {{0}} is already configured in NavigationService", key));

                if (_pages.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == pageType).Key}");
                }

                _pages.Add(key, pageType);
            }
        }

        public string GetNameOfRegisteredPage(Type page)
        {
            lock (_pages)
            {
                if (_pages.ContainsValue(page))
                    return _pages.FirstOrDefault(p => p.Value == page).Key;
                throw new ArgumentException($"The page '{page.Name}' is unknown by the NavigationService");
            }
        }

        private void RegisterFrameEvents()
        {
            if (frame != null)
            {
                frame.Navigated += Frame_Navigated;
                frame.NavigationFailed += Frame_NavigationFailed;
            }
        }

        private void UnregisterFrameEvents()
        {
            if (frame != null)
            {
                frame.Navigated -= Frame_Navigated;
                frame.NavigationFailed -= Frame_NavigationFailed;
            }
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(sender, e);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            Navigated?.Invoke(sender, e);
        }
    }
}
