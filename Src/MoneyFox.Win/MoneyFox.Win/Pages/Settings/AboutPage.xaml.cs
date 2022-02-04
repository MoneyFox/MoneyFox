using MoneyFox.Win.ViewModels.About;

namespace MoneyFox.Win.Pages.Settings
{
    public sealed partial class AboutPage
    {
        private AboutViewModel ViewModel => (AboutViewModel)DataContext;

        public AboutPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.AboutVm;
        }
    }
}