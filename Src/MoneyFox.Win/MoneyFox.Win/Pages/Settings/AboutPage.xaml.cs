namespace MoneyFox.Win.Pages.Settings;

using ViewModels.About;

public sealed partial class AboutPage
{
    public AboutPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.AboutVm;
    }

    private AboutViewModel ViewModel => (AboutViewModel)DataContext;
}
