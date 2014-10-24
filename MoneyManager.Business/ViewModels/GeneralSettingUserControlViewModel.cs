using System;
using System.Collections.Generic;
using MoneyManager.Business.Src;

namespace MoneyManager.Business.ViewModels
{
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