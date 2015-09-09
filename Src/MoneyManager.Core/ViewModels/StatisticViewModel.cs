using System;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.Manager;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticViewModel : BaseViewModel
    {
        private readonly StatisticManager statisticManager;
        private ObservableCollection<StatisticItem> categorySummary;
        private ObservableCollection<StatisticItem> monthlyCashFlow;
        private ObservableCollection<StatisticItem> monthlySpreading;

        public StatisticViewModel(StatisticManager statisticManager)
        {
            this.statisticManager = statisticManager;

            StartDate = DateTime.Now.Date.AddMonths(-1);
            EndDate = DateTime.Now.Date;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ObservableCollection<StatisticItem> MonthlyCashFlow
        {
            get
            {
                return monthlyCashFlow == null || !monthlyCashFlow.Any()
                    ? statisticManager.GetMonthlyCashFlow()
                    : monthlyCashFlow;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                monthlyCashFlow = value;
            }
        }

        public ObservableCollection<StatisticItem> MonthlySpreading
        {
            get
            {
                return monthlySpreading == null || !monthlySpreading.Any()
                    ? statisticManager.GetSpreading()
                    : monthlySpreading;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                monthlySpreading = value;
            }
        }

        public ObservableCollection<StatisticItem> CategorySummary
        {
            get
            {
                return categorySummary == null || !categorySummary.Any()
                    ? statisticManager.GetCategorySummary(StartDate, EndDate)
                    : categorySummary;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                categorySummary = value;
            }
        }

        public string Title => Strings.StatisticTitle + " " + StartDate.ToString("d") +
                               " - " +
                               EndDate.ToString("d");

        public void SetDefaultCashFlow()
        {
            MonthlyCashFlow = statisticManager.GetMonthlyCashFlow();
        }

        public void SetDefaultSpreading()
        {
            MonthlySpreading = statisticManager.GetSpreading();
        }

        public void SetCustomCashFlow()
        {
            MonthlyCashFlow = statisticManager.GetMonthlyCashFlow(StartDate, EndDate);
        }

        public void SetCustomSpreading()
        {
            MonthlySpreading = statisticManager.GetSpreading(StartDate, EndDate);
        }

        public void SetCagtegorySummary()
        {
            CategorySummary = statisticManager.GetCategorySummary(StartDate, EndDate);
        }
    }
}