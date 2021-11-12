using System;
using System.Collections.Concurrent;
using System.Linq;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp.Services
{
    public class NavigationService : INavigationService
    {
        private static readonly ConcurrentDictionary<Type, Type> viewModelMap = new ConcurrentDictionary<Type, Type>();

        static NavigationService()
        {
            MainViewId = ApplicationView.GetForCurrentView().Id;
        }

        public static int MainViewId { get; }

        public Frame? Frame { get; private set; }

        public bool CanGoBack => Frame?.CanGoBack ?? false;

        public bool GoBack()
        {
            if(CanGoBack)
            {
                Frame?.GoBack();
                return true;
            }

            return false;
        }

        public bool GoForward()
        {
            if(Frame != null && Frame.CanGoForward)
            {
                Frame.GoForward();
                return true;
            }

            return false;
        }

        public void Initialize(object frame) => Frame = (Frame)frame;

        public bool Navigate<TViewModel>(object? parameter = null) => Navigate(typeof(TViewModel), parameter);

        public bool Navigate(Type viewModelType, object? parameter = null)
        {
            if(Frame == null)
            {
                throw new InvalidOperationException("Navigation frame not initialized.");
            }

            return Frame.Navigate(GetView(viewModelType), parameter);
        }

        public static void Register<TViewModel, TView>() where TView : Page
        {
            if(!viewModelMap.TryAdd(typeof(TViewModel), typeof(TView)))
            {
                throw new InvalidOperationException($"ViewModel already registered '{typeof(TViewModel).FullName}'");
            }
        }

        public static Type GetView<TViewModel>() => GetView(typeof(TViewModel));

        public static Type GetView(Type viewModel)
        {
            if(viewModelMap.TryGetValue(viewModel, out var view))
            {
                return view;
            }

            throw new InvalidOperationException($"View not registered for ViewModel '{viewModel.FullName}'");
        }

        public static Type GetViewModel(Type view)
        {
            var type = viewModelMap.Where(r => r.Value == view).Select(r => r.Key).FirstOrDefault();
            if(type == null)
            {
                throw new InvalidOperationException($"View not registered for ViewModel '{view.FullName}'");
            }

            return type;
        }
    }
}