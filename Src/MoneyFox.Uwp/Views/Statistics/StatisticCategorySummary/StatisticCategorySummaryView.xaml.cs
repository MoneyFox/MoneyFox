#nullable enable
using MoneyFox.Application.Resources;
using MoneyFox.Uwp.ViewModels.Statistic.StatisticCategorySummary;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MoneyFox.Uwp.Views.Statistics.StatisticCategorySummary
{
    public sealed partial class StatisticCategorySummaryView
    {
        public StatisticCategorySummaryViewModel ViewModel => (StatisticCategorySummaryViewModel)DataContext;

        public override string Header => Strings.CategorySummaryTitle;

        public StatisticCategorySummaryView()
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