using System;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;

namespace MoneyFox.Business.ViewModels
{
    public interface ISettingsPersonalizationViewModel
    {
        int SelectedIndex { get; set; }
    }

    public class SettingsPersonalizationViewModel : BaseViewModel, ISettingsPersonalizationViewModel
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