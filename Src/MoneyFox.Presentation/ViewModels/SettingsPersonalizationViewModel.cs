using System;
using System.Collections.Generic;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;

namespace MoneyFox.Presentation.ViewModels
{
    public interface ISettingsPersonalizationViewModel : IBaseViewModel
    {
        /// <summary>
        ///     The Currently selected index.
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        ///     Available Themes
        /// </summary>
        List<string> ThemeItems { get; }
    }

    public class SettingsPersonalizationViewModel : BaseViewModel, ISettingsPersonalizationViewModel
    {
        private readonly ISettingsFacade settingsFacade;

        public SettingsPersonalizationViewModel(ISettingsFacade settingsFacade)
        {
            this.settingsFacade = settingsFacade;
        }

        /// <inheritdoc />
        public int SelectedIndex
        {
            get => (int) settingsFacade.Theme;
            set
            {
                var theme = (AppTheme)Enum.ToObject(typeof(AppTheme), value);
                settingsFacade.Theme = theme;
                RaisePropertyChanged();
            }
        }

        /// <inheritdoc />
        public List<string> ThemeItems => new List<string>
        {
            Strings.ThemeDarkLabel,
            Strings.ThemeLightLabel
        };
    }
}