namespace MoneyFox.Views.About;

using Ui;
using ViewModels.About;

public partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AboutViewModel>();
    }
}
