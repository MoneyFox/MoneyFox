using MoneyFox.Win.ViewModels.About;

namespace MoneyFox.Win.Pages.Settings
{
    public sealed partial class AboutView
    {
        private AboutViewModel ViewModel => (AboutViewModel)DataContext;

        public AboutView()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.AboutVm;
        }
    }
}