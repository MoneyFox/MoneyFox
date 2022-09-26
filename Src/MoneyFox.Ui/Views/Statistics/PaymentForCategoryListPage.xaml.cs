namespace MoneyFox.Ui.Views.Statistics;

using MoneyFox.Core.Resources;
using ViewModels.Statistics;

public partial class PaymentForCategoryListPage : ContentPage
{
    public PaymentForCategoryListPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<PaymentForCategoryListViewModel>();
        var doneItem = new ToolbarItem
        {
            Command = new Command(async () => await CloseAsync()),
            Text = Strings.DoneLabel,
            Priority = 1,
            Order = ToolbarItemOrder.Primary
        };

        ToolbarItems.Add(doneItem);
    }

    private async Task CloseAsync()
    {
        await Navigation.PopModalAsync();
    }
}
