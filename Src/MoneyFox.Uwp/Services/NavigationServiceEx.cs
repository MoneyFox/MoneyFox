using MoneyFox.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Services
{
    public class NavigationServiceEx : INavigationService
    {
        public event NavigatedEventHandler Navigated;

        public event NavigationFailedEventHandler NavigationFailed;

        private readonly Dictionary<string, Type> pages = new Dictionary<string, Type>();

        private Frame frame;
        private object lastParamUsed;

        public Frame Frame
        {
            get
            {
                if(frame == null)
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

        public void GoBack()
        {
            if(CanGoBack)
                Frame.GoBack();
        }

        public void GoForward()
        {
            Frame.GoForward();
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            Type page;
            lock(pages)
            {
                if(!pages.TryGetValue(pageKey, out page))
                    throw new ArgumentException(string.Format("Page Not Found. Key: {0}", pageKey), nameof(pageKey));
            }

            if(Frame.Content?.GetType() != page || parameter != null && !parameter.Equals(lastParamUsed))
            {
                bool navigationResult = Frame.Navigate(page, parameter);
                if(navigationResult)
                    lastParamUsed = parameter;
            }
        }

        public string CurrentPageKey { get; }

        public void Configure(string key, Type pageType)
        {
            lock(pages)
            {
                if(pages.ContainsKey(key))
                    throw new ArgumentException(string.Format("Key is not in page collection. Key: {0}", key));

                if(pages.Any(p => p.Value == pageType))
                    throw new ArgumentException(string.Format("Type Already Configured. Type: {0}", pages.First(p => p.Value == pageType).Key));

                pages.Add(key, pageType);
            }
        }

        public string GetNameOfRegisteredPage(Type page)
        {
            lock(pages)
            {
                if(pages.ContainsValue(page))
                    return pages.FirstOrDefault(p => p.Value == page).Key;

                throw new ArgumentException($"ExceptionNavigationServiceExPageUnknown. Page name: {page.Name}");
            }
        }

        private void RegisterFrameEvents()
        {
            if(frame != null)
            {
                frame.Navigated += Frame_Navigated;
                frame.NavigationFailed += Frame_NavigationFailed;
            }
        }

        private void UnregisterFrameEvents()
        {
            if(frame != null)
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

        public void NavigateToModal(string pageKey) => NavigateTo(pageKey);

        public void NavigateToModal(string pageKey, object parameter) => NavigateTo(pageKey, parameter);
    }
}
