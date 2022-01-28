using CommonServiceLocator;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using MoneyFox.Core._Pending_.Exceptions;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Win.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace MoneyFox.Win.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly KeyboardAccelerator altLeftKeyboardAccelerator =
            BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);

        private readonly KeyboardAccelerator backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool isBackEnabled;
        private IList<KeyboardAccelerator> keyboardAccelerators;
        private NavigationView navigationView;
        private NavigationViewItem selected;
        private ICommand loadedCommand;
        private ICommand itemInvokedCommand;

        private readonly INavigationService navigationService;

        public ShellViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            LoadMenuItems();
        }

        private ObservableCollection<MenuItem> menuItems;
        public ObservableCollection<MenuItem> MenuItems => menuItems;

        public void LoadMenuItems()
        {
            menuItems = new ObservableCollection<MenuItem>();

            //menuItems.Add(new MenuItem { Name = "Book Flight", Icon = "\uE709", ViewModelType = nameof(BookFlightViewModel) });
            //menuItems.Add(new MenuItem { Name = "Search", Icon = "\uE721", ViewModelType = nameof(SearchViewModel) });
        }

        public bool IsBackEnabled
        {
            get => isBackEnabled;
            set => SetProperty(ref isBackEnabled, value);
        }


        public WinUI.NavigationViewItem Selected
        {
            get => selected;
            set => SetProperty(ref selected, value);
        }

        public ICommand LoadedCommand => loadedCommand ??= new AsyncRelayCommand(OnLoadedAsync);

        public ICommand ItemInvokedCommand => itemInvokedCommand ??=
            new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);

        //public RelayCommand<PaymentType> GoToPaymentCommand =>
        //    new RelayCommand<PaymentType>(t => NavigationService.Navigate<AddPaymentViewModel>(t));

        public void Initialize(Frame frame,
            WinUI.NavigationView navigationView,
            IList<KeyboardAccelerator> keyboardAccelerators)
        {
            Logger.Debug("Is NavigationService available: {isAvailable}.", navigationService != null);

            if(navigationService == null)
            {
                return;
            }

            this.navigationView = navigationView;
            this.keyboardAccelerators = keyboardAccelerators;

            //frame.Navigated += Frame_Navigated;
            //frame.NavigationFailed += Frame_NavigationFailed;

            navigationService.Initialize(frame);
            this.navigationView.BackRequested += OnBackRequested;

            CoreWindow.GetForCurrentThread().PointerPressed += On_PointerPressed;
        }

        private void On_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            bool isXButton1Pressed = e.CurrentPoint.Properties.PointerUpdateKind == PointerUpdateKind.XButton1Pressed;

            if(isXButton1Pressed)
            {
                e.Handled = navigationService.GoBack();
            }

            bool isXButton2Pressed = e.CurrentPoint.Properties.PointerUpdateKind == PointerUpdateKind.XButton2Pressed;

            if(isXButton2Pressed)
            {
                e.Handled = navigationService.GoForward();
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
                //NavigationService.Navigate<WindowsSettingsViewModel>();

                return;
            }

            WinUI.NavigationViewItem item = navigationView?.MenuItems
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

            //string pageString = (string)item.GetValue(NavHelper.NavigateToProperty);
            //NavigationService.Navigate(GetTypeByString(pageString));
        }

        //private Type GetTypeByString(string pageString) =>
        //    pageString switch
        //    {
        //        "AccountListViewModel" => typeof(AccountListViewModel),
        //        "StatisticSelectorViewModel" => typeof(StatisticSelectorViewModel),
        //        "CategoryListViewModel" => typeof(CategoryListViewModel),
        //        "BackupViewModel" => typeof(BackupViewModel),
        //        _ => throw new PageNotFoundException(pageString)
        //    };

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args) =>
            navigationService.GoBack();

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => throw e.Exception;

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = navigationService.CanGoBack;

            //Selected = navigationView?.MenuItems
            //    .OfType<WinUI.NavigationViewItem>()
            //    .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        //private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        //{
        //    Type pageType = GetTypeByString(menuItem.GetValue(NavHelper.NavigateToProperty) as string ?? "");
        //    return pageType == sourcePageType;
        //}

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key,
            VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator { Key = key };
            if(modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender,
            KeyboardAcceleratorInvokedEventArgs args) => args.Handled = true;



        public class MenuItem
        {
            public string Name { get; set; }
            public string Icon { get; set; }
            public string ViewModelType { get; set; }
        }
    }
}