using Windows.UI.Xaml.Navigation;
using MoneyFox.Presentation.ViewModels;

namespace MoneyFox.Uwp.Views
{
    public sealed partial class EditAccountView
    {
        private EditAccountViewModel ViewModel => DataContext as EditAccountViewModel;

        public EditAccountView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null) ViewModel.AccountId = (int)e.Parameter;
        }
    }
}
