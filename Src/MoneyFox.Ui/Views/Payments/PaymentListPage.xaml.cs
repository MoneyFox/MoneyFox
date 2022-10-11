namespace MoneyFox.Ui.Views.Payments;

using CommunityToolkit.Maui.Views;
using Popups;
using ViewModels.Payments;

[QueryProperty(name: "AccountId", queryId: "accountId")]
public partial class PaymentListPage : ContentPage
{
    public PaymentListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<PaymentListViewModel>();
    }

    private PaymentListViewModel ViewModel => (PaymentListViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.OnAppearingAsync(accountId);
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
