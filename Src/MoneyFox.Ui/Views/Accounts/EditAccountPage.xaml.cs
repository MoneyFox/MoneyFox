namespace MoneyFox.Ui.Views.Accounts;

[QueryProperty(name: "AccountId", queryId: "accountId")]
public partial class EditAccountPage
{
    public EditAccountPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditAccountViewModel>();
    }

    private EditAccountViewModel ViewModel => (EditAccountViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync(accountId);
    }

#pragma warning disable S2376 // Write-only properties should not be used
    private int accountId;
    public string AccountId
    {
        set => accountId = Convert.ToInt32(Uri.UnescapeDataString(value));
    }
#pragma warning restore S2376 // Write-only properties should not be used
}
