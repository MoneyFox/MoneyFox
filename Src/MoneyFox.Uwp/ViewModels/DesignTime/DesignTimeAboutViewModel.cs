using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeAboutViewModel : IAboutViewModel
    {
        public AsyncCommand GoToWebsiteCommand { get; }
        public AsyncCommand SendMailCommand { get; }
        public RelayCommand RateAppCommand { get; }
        public AsyncCommand GoToRepositoryCommand { get; }
        public AsyncCommand GoToTranslationProjectCommand { get; }
        public AsyncCommand GoToDesignerTwitterAccountCommand { get; }
        public AsyncCommand GoToContributionPageCommand { get; }
        public string Version { get; } = "0.4.5";
        public string Website { get; } = "www.foo.ch";
        public string SupportMail { get; } = "foo@me.ch";
    }
}
