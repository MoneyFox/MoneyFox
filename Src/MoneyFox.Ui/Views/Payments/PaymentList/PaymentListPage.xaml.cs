namespace MoneyFox.Ui.Views.Payments.PaymentList;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

[QueryProperty(name: "AccountId", queryId: "accountId")]
public partial class PaymentListPage: IBindablePage
{
    public PaymentListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<PaymentListViewModel>();
    }

    private PaymentListViewModel ViewModel => (PaymentListViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.InitializeAsync().GetAwaiter().GetResult();
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }

    private void ShowFilterPopup(object sender, EventArgs e)
    {
        var popup = new FilterPopup();
        Shell.Current.ShowPopup(popup);
    }
}
