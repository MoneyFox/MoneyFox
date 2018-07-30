using System;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsPersonalizationViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;

        public SettingsPersonalizationViewModel(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public string HeaderContent => Strings.DarkModeHeader;
        public string Title => Strings.PersonalizationTitle;

        private string onContent = Strings.OnToggleLabel;
        public string OnContent
        {
            get
            {
                return onContent;
            }
            set
            {
                onContent = value;
                RaisePropertyChanged();
            }
        }
        private string offContent = Strings.OffToggleLabel;
        public string OffContent
        {
            get
            {
                return offContent;
            }
            set
            {
                offContent = value;
                RaisePropertyChanged();
            }
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
                    OnContent = Strings.OnRestartNeededLabel;
                }
                else
                {
                    theme = AppTheme.Light;
                    OffContent = Strings.OffRestartNeededLabel;
                }
                settingsManager.Theme = theme;
                RaisePropertyChanged();
            }
        }
    }
}