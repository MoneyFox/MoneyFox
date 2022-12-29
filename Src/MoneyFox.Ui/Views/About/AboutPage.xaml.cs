namespace MoneyFox.Ui.Views.About;

public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AboutViewModel>();
    }
}
