#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class StatisticViewModel : ViewModelBase {
        private ObservableCollection<StatisticItem> _categorySummary;
        private ObservableCollection<StatisticItem> _monthlyCashFlow;
        private ObservableCollection<StatisticItem> _monthlySpreading;

        public StatisticViewModel() {
            StartDate = DateTime.Now.Date.AddMonths(-1);
            EndDate = DateTime.Now.Date;
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ObservableCollection<StatisticItem> MonthlyCashFlow {
            get {
                return _monthlyCashFlow == null || !_monthlyCashFlow.Any()
                    ? StatisticLogic.GetMonthlyCashFlow()
                    : _monthlyCashFlow;
            }
            set {
                if (value == null) {
                    return;
                }
                _monthlyCashFlow = value;
            }
        }

        public ObservableCollection<StatisticItem> MonthlySpreading {
            get {
                return _monthlySpreading == null || !_monthlySpreading.Any()
                    ? StatisticLogic.GetSpreading()
                    : _monthlySpreading;
            }
            set {
                if (value == null) {
                    return;
                }
                _monthlySpreading = value;
            }
        }

        public ObservableCollection<StatisticItem> CategorySummary {
            get {
                return _categorySummary == null || !_categorySummary.Any()
                    ? StatisticLogic.GetCategorySummary(StartDate, EndDate)
                    : _categorySummary;
            }
            set {
                if (value == null) {
                    return;
                }
                _categorySummary = value;
            }
        }

        public string Title {
            get {
                return Translation.GetTranslation("StatistikTitle") + " " + StartDate.ToString("d") +
                       " - " +
                       EndDate.ToString("d");
            }
        }

        public void SetDefaultCashFlow() {
            MonthlyCashFlow = StatisticLogic.GetMonthlyCashFlow();
        }

        public void SetDefaultSpreading() {
            MonthlySpreading = StatisticLogic.GetSpreading();
        }

        public void SetCustomCashFlow() {
            MonthlyCashFlow = StatisticLogic.GetMonthlyCashFlow(StartDate, EndDate);
        }

        public void SetCustomSpreading() {
            MonthlySpreading = StatisticLogic.GetSpreading(StartDate, EndDate);
        }

        public void SetCagtegorySummary() {
            CategorySummary = StatisticLogic.GetCategorySummary(StartDate, EndDate);
        }
    }
}