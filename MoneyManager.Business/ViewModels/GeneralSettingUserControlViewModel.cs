using System;
using System.Collections.Generic;

namespace MoneyManager.Business.ViewModels
{
    internal class GeneralSettingUserControlViewModel
    {
        public List<String> LanguageList
        {
            get { return LanguageHelper.GetSupportedLanguages(); }
        }

        public string SelectedValue
        {
            get { return LanguageHelper.GetPrimaryLanguage(); }
            set { LanguageHelper.SetPrimaryLanguage(value); }
        }
    }
}