namespace MoneyFox.Ui;

public partial class MainPage
{
    public MainPage(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();
        BindingContext = mainPageViewModel;
    }

    private MainPageViewModel ViewModel => (MainPageViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.DashboardViewModel.OnNavigatedBackAsync(null);
    }
}
