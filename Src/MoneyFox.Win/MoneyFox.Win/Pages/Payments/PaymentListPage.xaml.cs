namespace MoneyFox.Win.Pages.Payments;

using System.Globalization;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using ViewModels.Payments;

public sealed partial class PaymentListPage
{
    public PaymentListPage()
    {
        InitializeComponent();
        DataContext = App.GetViewModel<PaymentListViewModel>();
    }

    public override bool ShowHeader => false;

    private PaymentListViewModel ViewModel => (PaymentListViewModel)DataContext;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter != null)
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
        var group = e.RowGroupHeader.CollectionViewGroup;
        var item = (PaymentViewModel)group.GroupItems[0];
        e.RowGroupHeader.PropertyValue = item.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture);
    }

    private void DataGrid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        if (((FrameworkElement)e.OriginalSource).DataContext is PaymentViewModel vm)
        {
            ViewModel.EditPaymentCommand.Execute(vm);
        }
    }

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is PaymentViewModel vm)
        {
            ViewModel.DeletePaymentCommand.Execute(vm);
        }
    }
}
