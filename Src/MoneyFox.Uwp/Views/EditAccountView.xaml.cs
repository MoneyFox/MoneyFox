using MoneyFox.Uwp.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditAccountView
    {
        private EditAccountViewModel ViewModel => (EditAccountViewModel) DataContext;

        public EditAccountView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.AccountId = (int)e.Parameter;
    }
}
