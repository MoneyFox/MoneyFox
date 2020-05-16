using MoneyFox.Domain;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs;

namespace MoneyFox.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppShell
    {
        private WindowsShellViewModel ViewModel => DataContext as WindowsShellViewModel;

        public AppShell()
        {
            InitializeComponent();
            DataContext = new WindowsShellViewModel();

            ViewModel.Initialize(ContentFrame, NavView, KeyboardAccelerators);
        }

        public Frame MainFrame => ContentFrame;

        private void NavView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if(args.InvokedItemContainer == openStatisticMenu)
            {
                FlyoutBase flyout = FlyoutBase.GetAttachedFlyout(openStatisticMenu);
                flyout.ShowAt(openStatisticMenu);
            }
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            FlyoutBase flyout = FlyoutBase.GetAttachedFlyout(SettingsButton);
            flyout.ShowAt(SettingsButton);
        }

        private void CategoriesMenuClick(object sender, RoutedEventArgs e)
        {
            WindowsShellViewModel.NavigationService.Navigate(ViewModelLocator.CategoryList);
        }

        private void BackupMenuClick(object sender, RoutedEventArgs e)
        {
            WindowsShellViewModel.NavigationService.Navigate(ViewModelLocator.Backup);
        }

        private void SettingsMenuClick(object sender, RoutedEventArgs e)
        {
            WindowsShellViewModel.NavigationService.Navigate(ViewModelLocator.Settings);
        }

        private void AboutMenuClick(object sender, RoutedEventArgs e)
        {
            WindowsShellViewModel.NavigationService.Navigate(ViewModelLocator.About);
        }

        private void CashFlowStatisticMenuClick(object sender, RoutedEventArgs e)
        {
            WindowsShellViewModel.NavigationService.Navigate(ViewModelLocator.StatisticCashFlow);
        }

        private void CategorySpreadingMenuClick(object sender, RoutedEventArgs e)
        {
            WindowsShellViewModel.NavigationService.Navigate(ViewModelLocator.StatisticCategorySpreading);
        }

        private void CategorySummaryMenuClick(object sender, RoutedEventArgs e)
        {
            WindowsShellViewModel.NavigationService.Navigate(ViewModelLocator.StatisticCategorySummary);
        }

        private async void AddPaymentItemTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await new AddPaymentView(PaymentType.Expense).ShowAsync();
        }
    }
}
