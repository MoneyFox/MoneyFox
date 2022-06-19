namespace MoneyFox.Views.SetupAssistant
{

    using ViewModels.SetupAssistant;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<WelcomeViewModel>();
        }

        private WelcomeViewModel ViewModel => (WelcomeViewModel)BindingContext;

        protected override async void OnAppearing()
        {
            await ViewModel.InitAsync();
        }
    }

}
