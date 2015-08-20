using System.Collections.Generic;
using MoneyManager.Business.Logic;
using MoneyManager.DataAccess.DataAccess;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class GeneralSettingViewModel
    {
        private readonly SettingDataAccess settingsDataAccess;

        public GeneralSettingViewModel(SettingDataAccess settingsDataAccess)
        {
            this.settingsDataAccess = settingsDataAccess;
        }
        
        /// <summary>
        ///     Gets or sets the default currency used in the system.
        /// </summary>
        public string DefaultCurrency => settingsDataAccess.DefaultCurrency;

        /// <summary>
        ///     returns all supported Languages
        /// </summary>
        public List<string> LanguageList => RegionLogic.GetSupportedLanguages();

        /// <summary>
        ///     gets or Sets the selected Language who is currently used
        /// </summary>
        public string SelectedLanguage
        {
            get { return RegionLogic.GetPrimaryLanguage(); }
            set { RegionLogic.SetPrimaryLanguage(value); }
        }
    }
}