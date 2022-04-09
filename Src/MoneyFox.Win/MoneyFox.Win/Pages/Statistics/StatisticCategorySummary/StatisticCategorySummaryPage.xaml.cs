namespace MoneyFox.Win.Pages.Statistics.StatisticCategorySummary;

using System.Linq;
using Core.Resources;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ViewModels.Statistics.StatisticCategorySummary;

public sealed partial class StatisticCategorySummaryPage
{
    public StatisticCategorySummaryPage()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.StatisticCategorySummaryVm;
    }

    public StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)DataContext;

    public override string Header => Strings.CategorySummaryTitle;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ViewModel.LoadedCommand.Execute(null);
    }

    private void CategorySummaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = e.AddedItems.FirstOrDefault();
        if (!(item is CategoryOverviewViewModel vm))
        {
            return;
        }

        ViewModel.SummaryEntrySelectedCommand.Execute(vm);
    }
}
