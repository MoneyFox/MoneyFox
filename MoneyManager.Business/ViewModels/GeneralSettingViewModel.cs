#region

using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class GeneralSettingViewModel {
        public List<String> LanguageList {
            get { return RegionLogic.GetSupportedLanguages(); }
        }

        public string SelectedValue {
            get { return RegionLogic.GetPrimaryLanguage(); }
            set { RegionLogic.SetPrimaryLanguage(value); }
        }


        public string DefaultCurrency {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency; }
        }
    }
}