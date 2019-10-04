using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeAboutViewModel : IAboutViewModel
    {
        public DesignTimeAboutViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <inheritdoc />
        public LocalizedResources Resources { get; }
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
