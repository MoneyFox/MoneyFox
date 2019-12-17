using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using MoneyFox.Presentation.Groups;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Uwp.Views.UserControls
{
    public partial class PaymentListUserControl
    {
        public PaymentListUserControl()
        {
            InitializeComponent();
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
                                                                                .Items.Select(x => (DateListGroupCollection<PaymentViewModel>) x)
                                                                                .FirstOrDefault(group => group.Any(x => x.IsCleared));

            if (selectedGroupCollection == null) return;

            PaymentListView.ScrollIntoView(selectedGroupCollection, ScrollIntoViewAlignment.Leading);
        }
    }
}
