using MoneyFox.Presentation.Utilities;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Presentation.Views
{
    public partial class AddAccountPage
    {
        private AddAccountViewModel ViewModel => BindingContext as AddAccountViewModel;

        public AddAccountPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AddAccountVm;
            ViewModel.InitializeCommand.ExecuteAsync().FireAndForgetSafe();
        }
    }
}
