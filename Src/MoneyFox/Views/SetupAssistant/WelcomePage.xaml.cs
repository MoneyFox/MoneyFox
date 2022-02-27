namespace MoneyFox.Views.SetupAssistant
{
    using CommonServiceLocator;
    using ViewModels.SetupAssistant;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

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