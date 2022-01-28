using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MoneyFox.Win.Services;
using NLog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private NavigationViewItem selected;

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
            //menuItems.Add(new MenuItem { Name = Strings.AccountsTitle, Icon = "\uE80F", ViewModelType = nameof(AccountListViewModel) });
            //menuItems.Add(new MenuItem { Name = Strings.StatisticsTitle, Icon = "\uE904", ViewModelType = nameof(StatisticSelectorViewModel) });
            //menuItems.Add(new MenuItem { Name = Strings.CategoriesTitle, Icon = "\uE8EC", ViewModelType = nameof(CategoryListViewModel) });
            //menuItems.Add(new MenuItem { Name = Strings.BackupTitle, Icon = "\uEA35", ViewModelType = nameof(BackupViewModel) });
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

        public ICommand SelectedPageChangedCommand => new RelayCommand<MenuItem>((value) => NavigateToMenuItem(value));
        private void NavigateToMenuItem(MenuItem item)
        {
            //switch(item.ViewModelType)
            //{
            //    case nameof(AccountListViewModel):
            //        navigationService.NavigateTo<AccountListViewModel>();
            //        break;
            //    case nameof(StatisticSelectorViewModel):
            //        navigationService.NavigateTo<StatisticSelectorViewModel>();
            //        break;
            //    case nameof(CategoryListViewModel):
            //        navigationService.NavigateTo<CategoryListViewModel>();
            //        break;
            //    case nameof(BackupViewModel):
            //        navigationService.NavigateTo<BackupViewModel>();
            //        break;
            //}
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