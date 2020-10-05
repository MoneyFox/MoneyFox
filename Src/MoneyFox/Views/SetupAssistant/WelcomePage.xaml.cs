using CommonServiceLocator;
using MoneyFox.ViewModels.SetupAssistant;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views.SetupAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        private WelcomeViewModel ViewModel => (WelcomeViewModel)BindingContext;

        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<WelcomeViewModel>();
        }

        protected override async void OnAppearing() => await ViewModel.InitAsync();
    }
}
