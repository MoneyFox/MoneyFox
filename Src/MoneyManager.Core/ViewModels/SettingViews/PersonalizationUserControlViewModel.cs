using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Core.Helpers;

namespace MoneyManager.Core.ViewModels.SettingViews
{
    public class PersonalizationUserControlViewModel : BaseViewModel
    {
        public bool IsDarkThemeEnabled
        {
            get { return Settings.SelectedTheme; }
            set { Settings.SelectedTheme = value; }
        }

        public PersonalizationUserControlViewModel()
        {
                 
        }

    }
}
