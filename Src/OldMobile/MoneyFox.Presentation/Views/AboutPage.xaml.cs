using MoneyFox.Application.Resources;

namespace MoneyFox.Presentation.Views
{
    public partial class AboutPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.AboutVm;

            Title = Strings.AboutTitle;
        }
    }
}
