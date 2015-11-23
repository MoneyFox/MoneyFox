using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels.Statistics;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels.Dialogs
{
    [ImplementPropertyChanged]
    public class SelectDateRangeDialogViewModel : BaseViewModel
    {
        private readonly StatisticViewModel statisticViewModel;

        public SelectDateRangeDialogViewModel(StatisticViewModel statisticViewModel)
        {
            this.statisticViewModel = statisticViewModel;

            StartDate = DateTime.Today.Date.AddDays(-1);
            EndDate = DateTime.Today;
        }

        /// <summary>
        ///     Startdate for the custom date range
        /// </summary>
        public DateTime StartDate
        {
            get { return statisticViewModel.StartDate; }
            set { statisticViewModel.StartDate = value; }
        }

        /// <summary>
        ///     Enddate for the custom date range
        /// </summary>
        public DateTime EndDate
        {
            get { return statisticViewModel.EndDate; }
            set { statisticViewModel.EndDate = value; }
        }

        /// <summary>
        ///     Reloads the statistic with the new daterange.
        /// </summary>
        public ICommand LoadStatisticCommand => new MvxCommand(LoadStatistic);

        private void LoadStatistic()
        {
            statisticViewModel.SetCashFlow();
            statisticViewModel.SetSpreading();
            statisticViewModel.SetCustomCategorySummary();
        }
    }
}
