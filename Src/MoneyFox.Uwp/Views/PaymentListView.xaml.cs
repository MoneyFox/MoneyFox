using MoneyFox.Uwp.ViewModels;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class PaymentListView
    {
        public override bool ShowHeader => false;

        private PaymentListViewModel ViewModel => (PaymentListViewModel) DataContext;

        public PaymentListView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                ViewModel.AccountId = (int)e.Parameter;
                ViewModel.InitializeCommand.Execute(null);
            }
        }

        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
        }

        private void dg_loadingRowGroup(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            var item = group.GroupItems[0] as PaymentViewModel;
            e.RowGroupHeader.PropertyValue = item.Date.ToString("D", CultureInfo.CurrentCulture);
        }
    }
}
