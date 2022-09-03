namespace MoneyFox.Views.Budget;

using Core.Resources;
using Ui;
using ViewModels.Budget;

public partial class AddBudgetPage
{
    public AddBudgetPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AddBudgetViewModel>();
        var cancelItem = new ToolbarItem
        {
            Command = new Command(async () => await Navigation.PopModalAsync()),
            Text = Strings.CancelLabel,
            Priority = -1,
            Order = ToolbarItemOrder.Primary
        };

        var saveItem = new ToolbarItem
        {
            Command = new Command(() => ViewModel.SaveBudgetCommand.Execute(null)),
            Text = Strings.SaveLabel,
            Priority = 1,
            Order = ToolbarItemOrder.Primary
        };

        ToolbarItems.Add(cancelItem);
        ToolbarItems.Add(saveItem);
    }

    private AddBudgetViewModel ViewModel => (AddBudgetViewModel)BindingContext;
}
