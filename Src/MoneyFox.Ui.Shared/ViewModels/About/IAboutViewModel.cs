using GalaSoft.MvvmLight.Command;

namespace MoneyFox.Ui.Shared.ViewModels.About
{
    public interface IAboutViewModel
    {
        /// <summary>
        /// Opens the webbrowser and loads to the apply solutions     website
        /// </summary>
        RelayCommand GoToWebsiteCommand { get; }

        /// <summary>
        /// Sends a feedback mail to the apply solutions support     mail address
        /// </summary>
        RelayCommand SendMailCommand { get; }

        /// <summary>
        /// Opens the store to rate the app.
        /// </summary>
        RelayCommand RateAppCommand { get; }

        /// <summary>
        /// Opens the webbrowser and loads repository page     on GitHub
        /// </summary>
        RelayCommand GoToRepositoryCommand { get; }

        /// <summary>
        /// Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        RelayCommand GoToTranslationProjectCommand { get; }

        /// <summary>
        /// Opens the webbrowser and loads the project on crowdin.
        /// </summary>
        RelayCommand GoToDesignerTwitterAccountCommand { get; }

        /// <summary>
        /// Opens the webbrowser loads the contribution page on Github.
        /// </summary>
        RelayCommand GoToContributionPageCommand { get; }

        /// <summary>
        /// Returns the Version of App
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Returns the apply solutions webite url from the     resource file
        /// </summary>
        string Website { get; }

        /// <summary>
        /// Returns the mailaddress for support cases from the     resource file
        /// </summary>
        string SupportMail { get; }
    }
}
