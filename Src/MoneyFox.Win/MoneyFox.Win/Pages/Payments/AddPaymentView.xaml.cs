using Microsoft.UI.Xaml.Navigation;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Payments;

namespace MoneyFox.Win.Pages.Payments
{
    public sealed partial class AddPaymentView
    {
        public override string Header => Strings.AddPaymentTitle;

        public AddPaymentViewModel ViewModel => (AddPaymentViewModel)DataContext;

        public AddPaymentView()
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
}