using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using MoneyFox.Win.ViewModels.Payments;
using System.Globalization;

namespace MoneyFox.Win.Pages.Payments
{
    public sealed partial class PaymentListPage
    {
        public override bool ShowHeader => false;

        private PaymentListViewModel ViewModel => (PaymentListViewModel)DataContext;

        public PaymentListPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.PaymentListVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                ViewModel.AccountId = (int)e.Parameter;
            }
        }

        private void OpenFilterFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void DataGrid_LoadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            var item = (PaymentViewModel)group.GroupItems[0];
            e.RowGroupHeader.PropertyValue = item.Date.ToString("D", CultureInfo.CurrentCulture);
        }

        private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if(((FrameworkElement)e.OriginalSource).DataContext is PaymentViewModel vm)
            {
                ViewModel.EditPaymentCommand.Execute(vm);
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            if(((FrameworkElement)sender).DataContext is PaymentViewModel vm)
            {
                ViewModel.DeletePaymentCommand.Execute(vm);
            }
        }
    }
}