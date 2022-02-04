using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MoneyFox.Core.Resources;
using MoneyFox.Win.ViewModels.Statistics.StatisticCategorySummary;
using System.Linq;

namespace MoneyFox.Win.Pages.Statistics.StatisticCategorySummary
{
    public sealed partial class StatisticCategorySummaryPage
    {
        public StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)DataContext;

        public override string Header => Strings.CategorySummaryTitle;

        public StatisticCategorySummaryPage()
        {
            InitializeComponent();
            DataContext = ViewModelLocator.StatisticCategorySummaryVm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.LoadedCommand.Execute(null);

        private void CategorySummaryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object item = e.AddedItems.FirstOrDefault();

            if(!(item is CategoryOverviewViewModel vm))
            {
                return;
            }

            ViewModel.SummaryEntrySelectedCommand.Execute(vm);
        }
    }
}