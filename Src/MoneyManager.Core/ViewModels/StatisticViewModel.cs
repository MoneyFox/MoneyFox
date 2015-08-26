using System;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.Logic;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;

namespace MoneyManager.Core.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticViewModel : BaseViewModel
    {
        private ObservableCollection<StatisticItem> categorySummary;
        private ObservableCollection<StatisticItem> monthlyCashFlow;
        private ObservableCollection<StatisticItem> monthlySpreading;

        public StatisticViewModel()
        {
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
                    ? StatisticLogic.GetMonthlyCashFlow()
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
                    ? StatisticLogic.GetSpreading()
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
                    ? StatisticLogic.GetCategorySummary(StartDate, EndDate)
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
            MonthlyCashFlow = StatisticLogic.GetMonthlyCashFlow();
        }

        public void SetDefaultSpreading()
        {
            MonthlySpreading = StatisticLogic.GetSpreading();
        }

        public void SetCustomCashFlow()
        {
            MonthlyCashFlow = StatisticLogic.GetMonthlyCashFlow(StartDate, EndDate);
        }

        public void SetCustomSpreading()
        {
            MonthlySpreading = StatisticLogic.GetSpreading(StartDate, EndDate);
        }

        public void SetCagtegorySummary()
        {
            CategorySummary = StatisticLogic.GetCategorySummary(StartDate, EndDate);
        }
    }
}