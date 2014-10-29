using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.Model;
using PropertyChanged;

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
