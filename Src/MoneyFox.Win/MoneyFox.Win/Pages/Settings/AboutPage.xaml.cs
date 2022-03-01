namespace MoneyFox.Win.Pages.Settings;

using ViewModels.About;

public sealed partial class AboutPage
{
    private AboutViewModel ViewModel => (AboutViewModel)DataContext;

    public AboutPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.AboutVm;
    }
}