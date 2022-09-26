namespace MoneyFox.Ui.Views.About;

using ViewModels.About;

public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AboutViewModel>();
    }
}
