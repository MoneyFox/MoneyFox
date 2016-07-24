using System;
using System.Collections.ObjectModel;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.StatisticDataProvider;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticCategorySummaryViewModel : StatisticViewModel, IDisposable
    {
        private readonly CategorySummaryDataProvider categorySummaryDataDataProvider;
        private readonly IUnitOfWork unitOfWork;

        public StatisticCategorySummaryViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            categorySummaryDataDataProvider = new CategorySummaryDataProvider(unitOfWork);
        }

        /// <summary>
        ///     Returns the Category Summary
        /// </summary>
        public ObservableCollection<StatisticItem> CategorySummary { get; set; }

        public void Dispose()
        {
            unitOfWork.Dispose();
        }

        protected override void Load()
        {
            CategorySummary = null;
            CategorySummary = GetCategorySummaryData();
        }

        private ObservableCollection<StatisticItem> GetCategorySummaryData()
            => new ObservableCollection<StatisticItem>(categorySummaryDataDataProvider.GetValues(StartDate, EndDate));
    }
}