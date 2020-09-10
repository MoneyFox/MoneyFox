using MoneyFox.Uwp.ViewModels;
using MoneyFox.Ui.Shared.Groups;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;

namespace MoneyFox.Uwp.Views.Statistics.StatisticCategorySummary
{
    public partial class PaymentListUserControl
    {
        public PaymentListUserControl()
        {
            InitializeComponent();
        }

        public CategoryOverviewViewModel ViewModel => (CategoryOverviewViewModel)DataContext;

        private void PaymentListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if(PaymentListView.Items == null || !PaymentListView.Items.Any())
                return;

            // Select first group with a cleared payment in it
            DateListGroupCollection<PaymentViewModel> selectedGroupCollection = PaymentListView
                                                                               .Items
                                                                               .Select(x => (DateListGroupCollection<PaymentViewModel>) x)
                                                                               .FirstOrDefault(group => group.Any(x => x.IsCleared));

            if(selectedGroupCollection == null) return;

            PaymentListView.ScrollIntoView(selectedGroupCollection, ScrollIntoViewAlignment.Leading);
        }

        private async void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var viewModel = (PaymentViewModel)e.ClickedItem;
            await new EditPaymentView(viewModel.Id).ShowAsync();
        }

        private async void EditPaymentClick(object sender, RoutedEventArgs e)
        {
            var viewModel = ((MenuFlyoutItem)sender).CommandParameter as PaymentViewModel;
            await new EditPaymentView(viewModel.Id).ShowAsync();
        }
    }
}
