namespace MoneyFox.Win.Pages.Payments;

using Core.Aggregates.Payments;
using Core.Resources;
using Microsoft.UI.Xaml.Navigation;
using ViewModels.Payments;

public sealed partial class AddPaymentPage
{
    public override string Header => Strings.AddPaymentTitle;

    public AddPaymentViewModel ViewModel => (AddPaymentViewModel)DataContext;

    public AddPaymentPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.AddPaymentVm;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var passedType = e.Parameter as PaymentType?;
        PaymentType type = passedType == null
            ? PaymentType.Expense
            : passedType.Value;

        ViewModel.PaymentType = type;
        ViewModel.InitializeCommand.Execute(null);
    }
}