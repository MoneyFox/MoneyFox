namespace MoneyFox.Views.SetupAssistant
{

    using ViewModels.SetupAssistant;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryIntroductionPage : ContentPage
    {
        public CategoryIntroductionPage()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<CategoryIntroductionViewModel>();
        }
    }

}
