﻿using CommonServiceLocator;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Domain;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Uwp.Helpers;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Accounts;
using MoneyFox.Uwp.ViewModels.Categories;
using MoneyFox.Uwp.ViewModels.DataBackup;
using MoneyFox.Uwp.ViewModels.Payments;
using MoneyFox.Uwp.ViewModels.Settings;
using MoneyFox.Uwp.ViewModels.Statistic;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;

#nullable enable
namespace MoneyFox.Uwp
{
    public class WindowsShellViewModel : ObservableObject
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly KeyboardAccelerator altLeftKeyboardAccelerator =
            BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);

        private readonly KeyboardAccelerator backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool isBackEnabled;
        private IList<KeyboardAccelerator>? keyboardAccelerators;
        private WinUI.NavigationView? navigationView;
        private WinUI.NavigationViewItem? selected;
        private ICommand? loadedCommand;
        private ICommand? itemInvokedCommand;

        public bool IsBackEnabled
        {
            get => isBackEnabled;
            set => SetProperty(ref isBackEnabled, value);
        }

        public static INavigationService NavigationService => ServiceLocator.Current.GetInstance<INavigationService>();

        public WinUI.NavigationViewItem? Selected
        {
            get => selected;
            set => SetProperty(ref selected, value);
        }

        public ICommand LoadedCommand => loadedCommand ??= new AsyncRelayCommand(OnLoadedAsync);

        public ICommand ItemInvokedCommand => itemInvokedCommand ??=
            new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);

        public RelayCommand<PaymentType> GoToPaymentCommand =>
            new RelayCommand<PaymentType>(t => NavigationService.Navigate<AddPaymentViewModel>(t));

        public void Initialize(Frame frame,
            WinUI.NavigationView navigationView,
            IList<KeyboardAccelerator> keyboardAccelerators)
        {
            Logger.Debug("Is NavigationService available: {isAvailable}.", NavigationService != null);

            if(NavigationService == null)
            {
                return;
            }

            this.navigationView = navigationView;
            this.keyboardAccelerators = keyboardAccelerators;

            frame.Navigated += Frame_Navigated;
            frame.NavigationFailed += Frame_NavigationFailed;

            NavigationService.Initialize(frame);
            this.navigationView.BackRequested += OnBackRequested;

            CoreWindow.GetForCurrentThread().PointerPressed += On_PointerPressed;
        }

        private void On_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            bool isXButton1Pressed = e.CurrentPoint.Properties.PointerUpdateKind == PointerUpdateKind.XButton1Pressed;

            if(isXButton1Pressed)
            {
                e.Handled = NavigationService.GoBack();
            }

            bool isXButton2Pressed = e.CurrentPoint.Properties.PointerUpdateKind == PointerUpdateKind.XButton2Pressed;

            if(isXButton2Pressed)
            {
                e.Handled = NavigationService.GoForward();
            }
        }

        private async Task OnLoadedAsync()
        {
            if(keyboardAccelerators == null)
            {
                return;
            }

            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            keyboardAccelerators.Add(altLeftKeyboardAccelerator);
            keyboardAccelerators.Add(backKeyboardAccelerator);
            await Task.CompletedTask;
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            Logger.Debug("Item invoked");

            if(args.IsSettingsInvoked)
            {
                Logger.Info("Navigate to settings");
                NavigationService.Navigate<WindowsSettingsViewModel>();

                return;
            }

            WinUI.NavigationViewItem? item = navigationView?.MenuItems
                                                           .OfType<WinUI.NavigationViewItem>()
                                                           .FirstOrDefault(
                                                               menuItem =>
                                                               {
                                                                   if(menuItem.Content is string content
                                                                      && args.InvokedItem is string invokedItem)
                                                                   {
                                                                       return content == invokedItem;
                                                                   }

                                                                   return false;
                                                               });

            if(item == null)
            {
                return;
            }

            string pageString = (string)item.GetValue(NavHelper.NavigateToProperty);
            NavigationService.Navigate(GetTypeByString(pageString));
        }

        private Type GetTypeByString(string pageString) =>
            pageString switch
            {
                "AccountListViewModel" => typeof(AccountListViewModel),
                "StatisticSelectorViewModel" => typeof(StatisticSelectorViewModel),
                "CategoryListViewModel" => typeof(CategoryListViewModel),
                "BackupViewModel" => typeof(BackupViewModel),
                _ => throw new PageNotFoundException(pageString)
            };

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args) =>
            NavigationService.GoBack();

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => throw e.Exception;

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;

            Selected = navigationView?.MenuItems
                                     .OfType<WinUI.NavigationViewItem>()
                                     .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            Type pageType = GetTypeByString(menuItem.GetValue(NavHelper.NavigateToProperty) as string ?? "");
            return pageType == sourcePageType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key,
            VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator {Key = key};
            if(modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender,
            KeyboardAcceleratorInvokedEventArgs args) => args.Handled = true;
    }
}