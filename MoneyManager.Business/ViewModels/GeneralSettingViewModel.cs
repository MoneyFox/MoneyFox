using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using PropertyChanged;
using System;
using System.Collections.Generic;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class GeneralSettingViewModel
    {
        public List<String> LanguageList
        {
            get { return LanguageLogic.GetSupportedLanguages(); }
        }

        public string SelectedValue
        {
            get { return LanguageLogic.GetPrimaryLanguage(); }
            set { LanguageLogic.SetPrimaryLanguage(value); }
        }


        public string DefaultCurrency
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>().DefaultCurrency; }
        }
    }
}