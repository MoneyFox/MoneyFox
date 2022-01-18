#nullable enable
using MoneyFox.Application.Resources;
using MoneyFox.Core;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Uwp.ViewModels.Payments;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views.Payments
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