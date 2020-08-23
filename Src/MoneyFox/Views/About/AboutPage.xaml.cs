using MoneyFox.Application.Resources;

namespace MoneyFox.Views.About
{
    public partial class AboutPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AboutViewModel;
        }
    }
}
