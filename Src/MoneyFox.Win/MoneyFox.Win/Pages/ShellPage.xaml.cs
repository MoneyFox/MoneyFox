using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using MoneyFox.Win.ViewModels;
using Windows.System;
using Windows.UI.Core;
using static MoneyFox.Win.ViewModels.ShellViewModel;

namespace MoneyFox.Win.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
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
        }

        private void AddPaymentItemTapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //MainContentFrame.NavigateToType(typeof(AddPaymentPage), null, navOptions);
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
                //MainContentFrame.NavigateToType(typeof(SettingsPage), null, navOptions);
            }
            else
            {
                var menuItem = args.InvokedItemContainer.DataContext as MenuItem;
                ViewModel.SelectedPageChangedCommand.Execute(menuItem);
            }
        }
    }
}
