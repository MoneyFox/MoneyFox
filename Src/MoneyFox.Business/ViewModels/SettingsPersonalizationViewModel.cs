using System;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsPersonalizationViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;

        public SettingsPersonalizationViewModel(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public bool ThemeToggled
        {
            get
            {
                if(settingsManager.Theme == AppTheme.Dark)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                AppTheme theme;
                if (value)
                {
                    theme = AppTheme.Dark;
                }
                else
                {
                    theme = AppTheme.Light;
                }
                settingsManager.Theme = theme;
                RaisePropertyChanged();
            }
        }
    }
}