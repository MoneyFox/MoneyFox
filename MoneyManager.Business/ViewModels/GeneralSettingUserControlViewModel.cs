using System;
using System.Collections.Generic;
using MoneyManager.Business.Src;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    internal class GeneralSettingUserControlViewModel
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