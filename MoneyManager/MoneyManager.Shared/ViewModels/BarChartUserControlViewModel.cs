using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using System.Collections.ObjectModel;

namespace MoneyManager.ViewModels
{
    public class BarChartUserControlViewModel : ViewModelBase
    {
        public ObservableCollection<StatisticItem> Chart
        {
            get { return ServiceLocator.Current.GetInstance<StatisticDataAccess>().MonthlyOverview; }
        }
    }
}