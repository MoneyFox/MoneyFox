namespace MoneyFox.Ui.Views.Statistics.CategoryProgression;

using Common.Navigation;
using CommunityToolkit.Maui.Views;

public partial class StatisticCategoryProgressionPage : IBindablePage
{
    public StatisticCategoryProgressionPage()
    {
        InitializeComponent();
    }

    private StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Application.Current!.MainPage!.ShowPopup(popup);
    }
}
