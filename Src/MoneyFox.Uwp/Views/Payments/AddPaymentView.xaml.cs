using MoneyFox.Domain;
using MoneyFox.Uwp.ViewModels.Payments;
using Windows.UI.Xaml.Navigation;

#nullable enable
namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class AddPaymentView
    {
        public AddPaymentView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.AddPaymentVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PaymentType? passedType = e.Parameter as PaymentType?;
            PaymentType type = passedType == null
                ? PaymentType.Expense
                : passedType.Value;

            ViewModel.Subscribe();
            ViewModel.PaymentType = type;
            ViewModel.InitializeCommand.Execute(null);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => ViewModel.Unsubscribe();

        public AddPaymentViewModel ViewModel => (AddPaymentViewModel)DataContext;

        public override bool ShowHeader => false;
    }
}
