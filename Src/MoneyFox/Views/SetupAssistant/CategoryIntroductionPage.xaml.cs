namespace MoneyFox.Views.SetupAssistant
{

    using CommonServiceLocator;
    using ViewModels.SetupAssistant;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

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
