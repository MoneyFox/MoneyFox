using System;
using System.Collections.Generic;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Logging;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
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

    public class SettingsPersonalizationViewModel : BaseNavigationViewModel, ISettingsPersonalizationViewModel
    {
        private readonly ISettingsManager settingsManager;

        public SettingsPersonalizationViewModel(ISettingsManager settingsManager,
                                                IMvxLogProvider logProvider,
                                                IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            this.settingsManager = settingsManager;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public List<string> ThemeItems => new List<string>
        {
            Strings.ThemeLightLabel,
            Strings.ThemeDarkLabel
        };
    }
}