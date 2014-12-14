#region

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
        private ObservableCollection<StatisticItem> _monthlyCashFlow;
        public ObservableCollection<StatisticItem> MonthlyCashFlow
        {
            get
            {
                _monthlyCashFlow = StatisticLogic.GetMonthlyCashFlow();
                return _monthlyCashFlow;
            }
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
    }
}