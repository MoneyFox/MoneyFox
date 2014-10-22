using System;
using System.Collections.Generic;
using MoneyManager.Src;

namespace MoneyManager.ViewModels
{
    public class GeneralSettingUserControlViewModel
    {
        public List<String> LanguageList
        {
            get { return LanguageHelper.GetSupportedLanguages(); }
        }

        public string SelectedValue
        {
            get { return LanguageHelper.GetPrimaryLanguage(); }
            set
            {
                LanguageHelper.SetPrimaryLanguage(value);
            }
        }
    }
}
