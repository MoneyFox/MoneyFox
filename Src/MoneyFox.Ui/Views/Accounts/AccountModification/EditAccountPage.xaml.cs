namespace MoneyFox.Ui.Views.Accounts.AccountModification;

using Common.Navigation;

[QueryProperty(name: "AccountId", queryId: "accountId")]
public partial class EditAccountPage: IBindablePage
{
    public EditAccountPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<EditAccountViewModel>();
    }

    private EditAccountViewModel ViewModel => (EditAccountViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.InitializeAsync(accountId).GetAwaiter().GetResult();
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }

#pragma warning disable S2376 // Write-only properties should not be used
    private int accountId;
    public string AccountId
    {
        set => accountId = Convert.ToInt32(Uri.UnescapeDataString(value));
    }
#pragma warning restore S2376 // Write-only properties should not be used
}
