namespace MoneyFox.Ui.Views.About;

using Common.Navigation;

public partial class AboutPage : IBindablePage
{
    public AboutPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<AboutViewModel>();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

#if WINDOWS
        var viewModel = (AboutViewModel)BindingContext;
        viewModel.OnNavigatedAsync(null).GetAwaiter().GetResult();
#endif
    }
}
