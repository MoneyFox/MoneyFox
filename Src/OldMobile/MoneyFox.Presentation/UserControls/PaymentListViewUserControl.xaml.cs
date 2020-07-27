using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace MoneyFox.Presentation.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentListViewUserControl : ContentView
    {
        public PaymentListViewUserControl()
        {
            InitializeComponent();

            PaymentList.On<Android>().SetIsFastScrollEnabled(true);
        }
    }
}