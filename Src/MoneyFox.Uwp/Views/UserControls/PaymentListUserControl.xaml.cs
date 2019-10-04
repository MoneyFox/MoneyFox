using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Presentation.Groups;

namespace MoneyFox.Uwp.Views.UserControls
{
    public partial class PaymentListUserControl
    {
        public PaymentListUserControl()
        {
            InitializeComponent();
        }

        private void EditPaymentViewModel(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            if (!(element.DataContext is PaymentViewModel payment))
            {
                return;
            }
            var viewmodel = DataContext as PaymentListViewModel;

            viewmodel?.EditPaymentCommand.Execute(payment);
        }

        private void DeletePaymentViewModel(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            if (!(element.DataContext is PaymentViewModel payment))
            {
                return;
            }
            (DataContext as PaymentListViewModel)?.DeletePaymentCommand.ExecuteAsync(payment).FireAndForgetSafeAsync();
        }

        private void PaymentViewModelList_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }

        private void PaymentListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (PaymentListView.Items == null || !PaymentListView.Items.Any()) return;

            // Select first group with a cleared payment in it
            DateListGroupCollection<PaymentViewModel> selectedGroupCollection = PaymentListView
                                                            .Items.Select(x => (DateListGroupCollection<PaymentViewModel>)x)
                                                            .FirstOrDefault(group => group.Any(x => x.IsCleared));

            if (selectedGroupCollection == null) return;

            PaymentListView.ScrollIntoView(selectedGroupCollection, ScrollIntoViewAlignment.Leading);
        }
    }
}
