using CommonServiceLocator;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using MoneyFox.Win.Pages.Payments;
using MoneyFox.Win.Pages.Settings;
using MoneyFox.Win.Services;
using MoneyFox.Win.ViewModels;
using Windows.System;
using Windows.UI.Core;
using static MoneyFox.Win.ViewModels.ShellViewModel;

namespace MoneyFox.Win.Pages
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel => (ShellViewModel)DataContext;

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.ShellVm;

            MainContentFrame.Navigated += MainContentFrame_Navigated;
            NavView.BackRequested += MenuNav_BackRequested;
            PointerPressed += OnMouseButtonClicked;

            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.Initialize(MainContentFrame);
        }

        private void AddPaymentItemTapped(object sender, TappedRoutedEventArgs e)
        {
            MainContentFrame.Navigate(typeof(AddPaymentPage));
        }

        private void MenuNav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if(MainContentFrame.CanGoBack)
            {
                MainContentFrame.GoBack();
            }
        }

        private void MainContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = MainContentFrame.CanGoBack;
        }

        private void OnMouseButtonClicked(object sender, PointerRoutedEventArgs e)
        {
            if(InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.XButton1) == CoreVirtualKeyStates.Down)
            {
                MainContentFrame.GoBack();
                e.Handled = true;
            }

            if(InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.XButton1) == CoreVirtualKeyStates.Down)
            {
                MainContentFrame.GoForward();
                e.Handled = true;
            }
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            if(args.IsSettingsInvoked)
            {
                MainContentFrame.NavigateToType(typeof(SettingsHostPage), null, navOptions);
            }
            else
            {
                var menuItem = args.InvokedItemContainer.DataContext as MenuItem;
                ViewModel.SelectedPageChangedCommand.Execute(menuItem);
            }
        }
    }
}
