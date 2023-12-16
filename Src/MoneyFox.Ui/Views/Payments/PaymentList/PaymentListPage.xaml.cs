namespace MoneyFox.Ui.Views.Payments.PaymentList;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class PaymentListPage : IBindablePage
{
    public PaymentListPage()
    {
        InitializeComponent();
     }

    private void ShowFilterPopup(object sender, EventArgs e)
    {
        var popup = new FilterPopup();
        Shell.Current.ShowPopup(popup);
    }
}
