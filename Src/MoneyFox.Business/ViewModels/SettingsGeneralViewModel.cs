using MoneyFox.Foundation.Interfaces;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
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

        public void ChangeLanguage()
        {
            //    if (shell.Language.Contains("de"))
            //        DLS.setIndex(0);
            //    if (shell.Language.Contains("de"))
            //        DLS.setIndex(1);
            //    if (shell.Language.Contains("es"))
            //        DLS.setIndex(2);
            //    if (shell.Language.Contains("pt"))
            //        DLS.setIndex(3);
            //   if (shell.Language.Contains("ru"))
            //        DLS.setIndex(4);
            //   if (shell.Language.Contains("cn"))
            //        DLS.setIndex(5);
            if (default_language_box.Index == 0)
            {
                DefaultLanguageSetter.ChosenLang = "de";
            }
            if (default_language_box.Index == 1)
            {
                DefaultLanguageSetter.ChosenLang = "de";
            }
            if (default_language_box.Index == 2)
            {
                DefaultLanguageSetter.ChosenLang = "es";
            }
            if (default_language_box.Index == 3)
            {
                DefaultLanguageSetter.ChosenLang = "pt";
            }
            if (default_language_box.Index == 4)
            {
                DefaultLanguageSetter.ChosenLang = "ru";
            }
            if (default_language_box.Index == 5)
            {
                DefaultLanguageSetter.ChosenLang = "cn";
            }
        }
    }
}