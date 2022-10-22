namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using CommunityToolkit.Maui.Views;
using MoneyFox.Ui.ViewModels.Statistics;
using MoneyFox.Ui.Views.Popups;

public partial class StatisticCategorySummaryPage
{
    public StatisticCategorySummaryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<StatisticCategorySummaryViewModel>();
        ViewModel.LoadedCommand.Execute(null);
    }

    private StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
