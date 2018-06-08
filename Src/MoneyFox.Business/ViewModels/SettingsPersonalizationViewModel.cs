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

        public int SelectedIndex
        {
            get => (int) settingsManager.Theme;
            set
            {
                var theme = (AppTheme)Enum.ToObject(typeof(AppTheme), value);
                settingsManager.Theme = theme;
                RaisePropertyChanged();
            }
        }
    }
}