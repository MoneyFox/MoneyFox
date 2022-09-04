namespace MoneyFox.Views.Payments;

using Core.Resources;
using Ui;
using ViewModels.Payments;

public partial class AddPaymentPage
{
    private int? defaultChargedAccountID;

    public AddPaymentPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddPaymentViewModel>();
        var cancelItem = new ToolbarItem
        {
            Command = new Command(async () => await Navigation.PopModalAsync()),
            Text = Strings.CancelLabel,
            Priority = -1,
            Order = ToolbarItemOrder.Primary
        };

        var saveItem = new ToolbarItem
        {
            Command = new Command(() => ViewModel.SaveCommand.Execute(null)),
            Text = Strings.SaveLabel,
            Priority = 1,
            Order = ToolbarItemOrder.Primary
        };

        ToolbarItems.Add(cancelItem);
        ToolbarItems.Add(saveItem);
    }

    public int? DefaultChargedAccountID
    {
        // Change to init setter when the project is upgraded to C# 9.
        set => defaultChargedAccountID = value;
    }

    private AddPaymentViewModel ViewModel => (AddPaymentViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync(defaultChargedAccountID);
    }
}
