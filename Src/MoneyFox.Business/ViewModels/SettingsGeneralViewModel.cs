using MoneyFox.Foundation.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using DefaultLanguageSetter;

namespace MoneyFox.Business.ViewModels
{
    public class SettingsGeneralViewModel : BaseViewModel
    {
        private readonly ISettingsManager settingsManager;

        public SettingsGeneralViewModel(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        //Command to actiate when language drop-down is used.
        public MvxCommand ChangeLanguageCommand => new MvxCommand(ChangeLanguage);

        public bool IsAutoBackupEnabled
        {
            get { return settingsManager.IsBackupAutouploadEnabled; }
            set
            {
                settingsManager.IsBackupAutouploadEnabled = value;
                RaisePropertyChanged();
            }
        }

        //boolean for changing the default language
        public bool IsDefaultLanguageChanged
        {
            get { return settingsManager.IsDefaultLanguageChosen; }
            set
            {
                settingsManager.IsDefaultLanguageChosen = value;
                RaisePropertyChanged();
            }
        }
        //The code which runs when the ChangeLanguage command occurs.
        public void ChangeLanguage()
        {
            DefaultLanguageSetter.LanguageSetter.LangSelected = true;

            //if (default_language_box.Index == 0)
            //{
            //    "de";
            //}
            //if (default_language_box.Index == 1)
            //{
            //    DefaultLanguageSetter.LanguageSetter.ChosenLang = "de";
            //}
            //if (default_language_box.Index == 2)
            //{
            //    DefaultLanguageSetter.LanguageSetter.ChosenLang = "es";
            //}
            //if (default_language_box.Index == 3)
            //{
            //    DefaultLanguageSetter.LanguageSetter.ChosenLang = "pt";
            //}
            //if (default_language_box.Index == 4)
            //{
            //    DefaultLanguageSetter.LanguageSetter.ChosenLang = "ru";
            //}
            //if (default_language_box.Index == 5)
            //{
            //    DefaultLanguageSetter.LanguageSetter.ChosenLang = "cn";
            //}
        }
    }
}