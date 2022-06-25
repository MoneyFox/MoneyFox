namespace MoneyFox.Win.Pages.Settings;

using ViewModels.About;

public sealed partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
        DataContext = App.GetViewModel<AboutViewModel>();
    }

    private AboutViewModel ViewModel => (AboutViewModel)DataContext;
}
