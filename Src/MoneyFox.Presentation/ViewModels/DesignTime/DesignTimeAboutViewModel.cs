using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;

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
        public RelayCommand GoToWebsiteCommand { get; }
        public RelayCommand SendMailCommand { get; }
        public RelayCommand RateAppCommand { get; }
        public RelayCommand GoToRepositoryCommand { get; }
        public RelayCommand GoToTranslationProjectCommand { get; }
        public RelayCommand GoToDesignerTwitterAccountCommand { get; }
        public RelayCommand GoToContributionPageCommand { get; }
        public string Version { get; } = "0.4.5";
        public string Website { get; } = "www.foo.ch";
        public string SupportMail { get; } = "foo@me.ch";
    }
}
