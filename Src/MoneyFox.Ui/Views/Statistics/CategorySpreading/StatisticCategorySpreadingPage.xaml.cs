namespace MoneyFox.Ui.Views.Statistics.CategorySpreading;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticCategorySpreadingPage: IBindablePage
{
    public StatisticCategorySpreadingPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticCategorySpreadingViewModel>();
    }

    private StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.IsActive = true;
        ViewModel.LoadedCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        ViewModel.IsActive = false;
    }

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
