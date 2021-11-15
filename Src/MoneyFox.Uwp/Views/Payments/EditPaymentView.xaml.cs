using MoneyFox.Uwp.ViewModels.Payments;
using Windows.UI.Xaml.Navigation;

#nullable enable
namespace MoneyFox.Uwp.Views.Payments
{
    public sealed partial class EditPaymentView : BaseView
    {
        private EditPaymentViewModel ViewModel => (EditPaymentViewModel)DataContext;

        public EditPaymentView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.EditPaymentVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = (PaymentViewModel)e.Parameter;
            ViewModel.InitializeCommand.Execute(vm.Id);
        }
    }
}