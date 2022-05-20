namespace MoneyFox.Views.SetupAssistant
{

    using CommonServiceLocator;
    using ViewModels.SetupAssistant;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryIntroductionPage : ContentPage
    {
        public CategoryIntroductionPage()
        {
            InitializeComponent();
            BindingContext = ServiceLocator.Current.GetInstance<CategoryIntroductionViewModel>();
        }
    }

}
