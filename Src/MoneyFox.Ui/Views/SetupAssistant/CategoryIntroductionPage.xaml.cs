namespace MoneyFox.Ui.Views.SetupAssistant;

using ViewModels.SetupAssistant;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CategoryIntroductionPage : ContentPage
{
    public CategoryIntroductionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<CategoryIntroductionViewModel>();
    }
}
