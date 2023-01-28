namespace MoneyFox.Ui.Views.Statistics.CategorySpreading;

using CommunityToolkit.Maui.Views;
using MoneyFox.Ui.Views.Popups;

public partial class StatisticCategorySpreadingPage
{
    public StatisticCategorySpreadingPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticCategorySpreadingViewModel>();
        ViewModel.LoadedCommand.Execute(null);
    }

    private StatisticCategorySpreadingViewModel ViewModel => (StatisticCategorySpreadingViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
