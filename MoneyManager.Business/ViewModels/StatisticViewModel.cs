#region

using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MoneyManager.Business.Logic;
using MoneyManager.Foundation.Model;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class StatisticViewModel : ViewModelBase
    {
        public ObservableCollection<StatisticItem> MonthlyCashFlow
        {
            get { return StatisticLogic.GetMonthlyCashFlow(); }
        }
    }
}