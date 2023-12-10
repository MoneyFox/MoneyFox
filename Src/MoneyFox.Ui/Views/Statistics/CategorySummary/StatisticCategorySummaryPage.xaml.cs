namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticCategorySummaryPage : IBindablePage
{
    public StatisticCategorySummaryPage()
    {
        InitializeComponent();
    }

    private StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
