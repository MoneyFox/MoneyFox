using Windows.UI.Xaml.Navigation;
using MoneyFox.Uwp.ViewModels;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditAccountView
    {
        public override string Header => ViewModelLocator.EditPaymentVm.Title;
        private EditAccountViewModel ViewModel => DataContext as EditAccountViewModel;

        public EditAccountView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) ViewModel.AccountId = (int) e.Parameter;
        }
    }
}
