namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;

public partial class SetupAccountPage : IBindablePage
{
    public SetupAccountPage()
    {
        InitializeComponent();
    }
    private SetupAccountsViewModel ViewModel => (SetupAccountsViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.MadeAccount();
        NextStepButton.IsVisible = ViewModel.HasAnyAccount;
    }
}
