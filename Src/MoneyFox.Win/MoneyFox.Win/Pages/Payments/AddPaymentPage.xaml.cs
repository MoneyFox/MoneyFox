namespace MoneyFox.Win.Pages.Payments;

using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using Core.Resources;
using Microsoft.UI.Xaml.Navigation;
using ViewModels.Payments;

public sealed partial class AddPaymentPage
{
    public AddPaymentPage()
    {
        InitializeComponent();
        DataContext = App.GetViewModel<AddPaymentViewModel>();
    }

    public override string Header => Strings.AddPaymentTitle;

    internal AddPaymentViewModel ViewModel => (AddPaymentViewModel)DataContext;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var passedType = e.Parameter as PaymentType?;
        var type = passedType == null ? PaymentType.Expense : passedType.Value;
        ViewModel.PaymentType = type;
        ViewModel.InitializeCommand.Execute(null);
    }
}
