namespace MoneyFox.Ui.Views.Payments.PaymentList;

using CommunityToolkit.Maui.Views;

[QueryProperty(name: "AccountId", queryId: "accountId")]
public partial class PaymentListPage : ContentPage
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
        ViewModel.InitializeAsync(accountId).GetAwaiter().GetResult();
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

#pragma warning disable S2376 // Write-only properties should not be used
    private int accountId;
    public string AccountId
    {
        set => accountId = Convert.ToInt32(Uri.UnescapeDataString(value));
    }
#pragma warning restore S2376 // Write-only properties should not be used
}
