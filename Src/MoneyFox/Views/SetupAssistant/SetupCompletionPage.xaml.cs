namespace MoneyFox.Views.SetupAssistant
{

    using CommonServiceLocator;
    using ViewModels.SetupAssistant;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupCompletionPage : ContentPage
    {
        public SetupCompletionPage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<SetupCompletionViewModel>();
        }
    }

}
