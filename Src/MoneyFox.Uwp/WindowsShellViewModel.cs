using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.Helpers;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.ViewModels.Settings;
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

namespace MoneyFox.Uwp
{
    public class WindowsShellViewModel : ViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly KeyboardAccelerator altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);

        private readonly KeyboardAccelerator backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool isBackEnabled;
        private IList<KeyboardAccelerator> keyboardAccelerators;
        private WinUI.NavigationView navigationView;
        private WinUI.NavigationViewItem selected;
        private ICommand loadedCommand;
        private ICommand itemInvokedCommand;

        public bool IsBackEnabled
        {
            get => isBackEnabled;
            set => Set(ref isBackEnabled, value);
        }

        public static NavigationService NavigationService => ServiceLocator.Current.GetInstance<NavigationService>();

        public WinUI.NavigationViewItem Selected
        {
            get => selected;
            set => Set(ref selected, value);
        }

        public ICommand LoadedCommand => loadedCommand ?? (loadedCommand = new AsyncCommand(OnLoadedAsync));

        public ICommand ItemInvokedCommand => itemInvokedCommand ?? (itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            Logger.Debug("Is NavigationService available: {isAvailable}.", NavigationService != null);

            this.navigationView = navigationView;
            this.keyboardAccelerators = keyboardAccelerators;
            NavigationService.Frame = frame;
            NavigationService.NavigationFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
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
                NavigationService.Navigate(nameof(WindowsSettingsViewModel));

                return;
            }

            WinUI.NavigationViewItem item = navigationView.MenuItems
                                                          .OfType<WinUI.NavigationViewItem>()
                                                          .FirstOrDefault(menuItem =>
                                                                          {
                                                                              if(menuItem.Content is string content
                                                                                 && args.InvokedItem is string invokedItem)
                                                                                  return content == invokedItem;

                                                                              return false;
                                                                          });

            if(item == null)
                return;

            var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;

            Logger.Info("Navigate to page key {key}", pageKey);
            NavigationService.Navigate(pageKey);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;

            Selected = navigationView.MenuItems
                                     .OfType<WinUI.NavigationViewItem>()
                                     .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            string navigatedPageKey = NavigationService.GetNameOfRegisteredPage(sourcePageType);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;

            return pageKey == navigatedPageKey;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator { Key = key };
            if(modifiers.HasValue)
                keyboardAccelerator.Modifiers = modifiers.Value;

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
        }
    }
}
