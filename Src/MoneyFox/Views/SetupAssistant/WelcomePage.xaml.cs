using CommonServiceLocator;
using MoneyFox.ViewModels.SetupAssistant;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views.SetupAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<WelcomeViewModel>();
        }

        private WelcomeViewModel ViewModel => (WelcomeViewModel)BindingContext;

        protected override async void OnAppearing() => await ViewModel.InitAsync();
    }
}