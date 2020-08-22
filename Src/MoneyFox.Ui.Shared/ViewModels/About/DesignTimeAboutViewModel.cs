using GalaSoft.MvvmLight.Command;

namespace MoneyFox.Ui.Shared.ViewModels.About
{
    public class DesignTimeAboutViewModel : IAboutViewModel
    {
        public RelayCommand GoToWebsiteCommand { get; } = null!;

        public RelayCommand SendMailCommand { get; } = null!;

        public RelayCommand RateAppCommand { get; } = null!;

        public RelayCommand GoToRepositoryCommand { get; } = null!;

        public RelayCommand GoToTranslationProjectCommand { get; } = null!;

        public RelayCommand GoToDesignerTwitterAccountCommand { get; } = null!;

        public RelayCommand GoToContributionPageCommand { get; } = null!;

        public string Version { get; } = "0.4.5";

        public string Website { get; } = "www.foo.ch";

        public string SupportMail { get; } = "foo@me.ch";
    }
}
