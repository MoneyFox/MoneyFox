using Xamarin.Forms;

namespace MoneyFox.Views
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.DashboardViewModel;
        }
    }
}