namespace MoneyFox.Ui.Views.Setup;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CategoryIntroductionPage : ContentPage
{
    public CategoryIntroductionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCategoryViewModel>();
    }
}
