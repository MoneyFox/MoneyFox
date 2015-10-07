using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectStatisticDialogViewModel : BaseViewModel
    {
        private readonly StatisticViewModel statisticViewModel;

        public SelectStatisticDialogViewModel(StatisticViewModel statisticViewModel)
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
            statisticViewModel.SetCustomCashFlow();
            statisticViewModel.SetCustomSpreading();
            statisticViewModel.SetCategorySummary();
        }
    }
}
