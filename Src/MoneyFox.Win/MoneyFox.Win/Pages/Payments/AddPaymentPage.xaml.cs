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
        PaymentType type = PaymentType.Expense;
        if (e.Parameter is PaymentType)
        {
            type = (PaymentType)e.Parameter;
        }
        else if (e.Parameter is int)
        {
            ViewModel.DefaultChargedAccountID = (int)e.Parameter;
        }

        ViewModel.PaymentType = type;
        ViewModel.InitializeCommand.Execute(null);
    }
}
