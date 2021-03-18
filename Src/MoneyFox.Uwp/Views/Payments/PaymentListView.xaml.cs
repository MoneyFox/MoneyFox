using Microsoft.Toolkit.Uwp.UI.Controls;
using MoneyFox.Ui.Shared.ViewModels.Payments;
using MoneyFox.Uwp.ViewModels.Payments;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

#nullable enable
namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class PaymentListView
    {
        public override bool ShowHeader => false;

        private PaymentListViewModel ViewModel => (PaymentListViewModel)DataContext;

        public PaymentListView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.PaymentListVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            if(e.Parameter != null)
            {
                ViewModel.AccountId = (int)e.Parameter;
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e) => ViewModel.Unsubscribe();

        private void OpenFilterFlyout(object sender, RoutedEventArgs e) => FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

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

        private void SortDataGrid(object sender, DataGridColumnEventArgs e)
        {
            var sortParameter = new SortParameter
            {
                Tag = e.Column.Tag.ToString(),
            };

            sortParameter.SortDirection = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending
                ? DataGridSortDirection.Ascending
                : DataGridSortDirection.Descending;
            e.Column.SortDirection = sortParameter.SortDirection;

            ViewModel.SortDataCommand.Execute(sortParameter);

            // Remove sorting indicators from other columns
            foreach(var dgColumn in PaymentGrid.Columns)
            {
                if(dgColumn.Tag == null || dgColumn.Tag.ToString() != e.Column.Tag.ToString())
                {
                    dgColumn.SortDirection = null;
                }
            }
        }
    }
}
