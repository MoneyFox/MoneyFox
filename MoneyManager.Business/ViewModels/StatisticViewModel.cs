using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
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
