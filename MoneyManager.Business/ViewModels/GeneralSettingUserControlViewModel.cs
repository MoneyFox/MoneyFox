using MoneyManager.Business.Src;
using PropertyChanged;
using System;
using System.Collections.Generic;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class GeneralSettingUserControlViewModel
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
    }
}