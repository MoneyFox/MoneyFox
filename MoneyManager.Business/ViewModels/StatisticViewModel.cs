#region

using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation.Model;
using PropertyChanged;
using Telerik.Charting;

#endregion

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticViewModel : ViewModelBase
    {
        public StatisticViewModel()
        {
            StartDate = DateTime.Now.Date.AddMonths(-1);
            EndDate = DateTime.Now.Date;
        }

        private ObservableCollection<StatisticItem> _monthlyCashFlow;
        public ObservableCollection<StatisticItem> MonthlyCashFlow
        {
            get { return _monthlyCashFlow ?? (_monthlyCashFlow = StatisticLogic.GetMonthlyCashFlow()); }
            set
            {
                if (value == null) return;
                _monthlyCashFlow = value;
            }
        }

        private ObservableCollection<StatisticItem> _monthlySpreading;
        public ObservableCollection<StatisticItem> MonthlySpreading
        {
            get
            {
                _monthlySpreading = StatisticLogic.GetSpreading();
                return _monthlySpreading;
            }
            set
            {
                if (value == null) return;
                _monthlySpreading = value;
            }
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public void SetCustomCashFlow()
        {
            MonthlyCashFlow = StatisticLogic.GetMonthlyCashFlow(StartDate, EndDate);
        }
    }
}