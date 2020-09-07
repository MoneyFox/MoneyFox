using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    public class NavigationService : INavigationService
    {
        public event NavigatedEventHandler Navigated;

        public event NavigationFailedEventHandler NavigationFailed;

        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();

        private Frame frame;
        private object lastParamUsed;

        static NavigationService()
        {
            MainViewId = ApplicationView.GetForCurrentView().Id;
        }

        static public int MainViewId { get; }

        public Frame Frame
        {
            get
            {
                if (frame == null)
                {
                    frame = (Frame)Window.Current.Content;
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
            Type page = GetPageForPageKey(pageKey);

            if(Frame.Content?.GetType() != page || parameter != null && !parameter.Equals(lastParamUsed))
            {
                bool navigationResult = Frame.Navigate(page, parameter, infoOverride);
                if(navigationResult)
                    lastParamUsed = parameter;

                return navigationResult;
            }

            return false;
        }

        private Type GetPageForPageKey(string pageKey)
        {
            Type page;
            lock(_pages)
            {
                if(!_pages.TryGetValue(pageKey, out page))
                {
                    throw new
                        ArgumentException($"Page not found: {pageKey}. Did you forget to call NavigationService.Configure?",
                                          nameof(pageKey));
                }
            }

            return page;
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

        public async Task<int> CreateNewViewAsync(string pageKey, object? parameter = null)
        {
            int viewId = 0;

            var newView = CoreApplication.CreateNewView();
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                viewId = ApplicationView.GetForCurrentView().Id;

                Type page = GetPageForPageKey(pageKey);
                var newFrame = new Frame();
                newFrame.Navigate(page, parameter);

                Window.Current.Content = newFrame;
                Window.Current.Activate();
            });

            if(await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId))
            {
                return viewId;
            }

            return 0;
        }

        public async Task CloseViewAsync()
        {
            int currentId = ApplicationView.GetForCurrentView().Id;
            await ApplicationViewSwitcher.SwitchAsync(MainViewId, currentId, ApplicationViewSwitchingOptions.ConsolidateViews);
        }
    }
}
