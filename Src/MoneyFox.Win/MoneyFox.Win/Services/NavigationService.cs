namespace MoneyFox.Win.Services;

using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using ViewModels.Accounts;

public class NavigationService : INavigationService
{
    private static readonly ConcurrentDictionary<Type, Type> ViewModelMap = new();

    public Frame? Frame { get; private set; }

    public bool CanGoBack => Frame?.CanGoBack ?? false;

    public bool GoBack()
    {
        if (CanGoBack)
        {
            Frame?.GoBack();

            return true;
        }

        return false;
    }

    public bool GoForward()
    {
        if (Frame != null && Frame.CanGoForward)
        {
            Frame.GoForward();

            return true;
        }

        return false;
    }

    public void Initialize(object frame)
    {
        Frame = (Frame)frame;
        Navigate<AccountListViewModel>();
    }

    public bool Navigate<TViewModel>(object parameter = null)
    {
        return Navigate(viewModelType: typeof(TViewModel), parameter: parameter);
    }

    public bool Navigate(Type viewModelType, object parameter = null)
    {
        if (Frame == null)
        {
            throw new InvalidOperationException("Navigation frame not initialized.");
        }

        return Frame.Navigate(sourcePageType: GetView(viewModelType), parameter: parameter);
    }

    public static void Register<TViewModel, TView>() where TView : Page
    {
        if (!ViewModelMap.TryAdd(key: typeof(TViewModel), value: typeof(TView)))
        {
            throw new InvalidOperationException($"ViewModel already registered '{typeof(TViewModel).FullName}'");
        }
    }

    public static Type GetView<TViewModel>()
    {
        return GetView(typeof(TViewModel));
    }

    public static Type GetView(Type viewModel)
    {
        if (ViewModelMap.TryGetValue(key: viewModel, value: out var view))
        {
            return view;
        }

        throw new InvalidOperationException($"View not registered for ViewModel '{viewModel.FullName}'");
    }

    public static Type GetViewModel(Type view)
    {
        var type = ViewModelMap.Where(r => r.Value == view).Select(r => r.Key).FirstOrDefault();

        return type ?? throw new InvalidOperationException($"View not registered for ViewModel '{view.FullName}'");
    }
}
