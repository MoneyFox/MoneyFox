namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using Common.Navigation;

public partial class StatisticCategorySummaryPage : IBindablePage
{
    public StatisticCategorySummaryPage()
    {
        InitializeComponent();
    }

    public StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)BindingContext;
}
