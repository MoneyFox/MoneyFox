namespace MoneyFox.Views.Statistics;

using CommunityToolkit.Maui.Views;
using Popups;
using ViewModels.Statistics;

public partial class StatisticCategoryProgressionPage : ContentPage
{
    public StatisticCategoryProgressionPage()
    {
        InitializeComponent();
        BindingContext = ViewModelLocator.StatisticCategoryProgressionViewModel;
    }

    private StatisticCategoryProgressionViewModel ViewModel => (StatisticCategoryProgressionViewModel)BindingContext;

    private void OpenFilterDialog(object sender, EventArgs e)
    {
        var popup = new DateSelectionPopup(dateFrom: ViewModel.StartDate, dateTo: ViewModel.EndDate);
        Shell.Current.ShowPopup(popup);
    }
}
