using System.Globalization;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeAboutViewModel : IAboutViewModel
    {
        public DesignTimeAboutViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <inheritdoc />
        public LocalizedResources Resources { get; }
        public MvxCommand GoToWebsiteCommand { get; }
        public MvxCommand SendMailCommand { get; }
        public MvxCommand RateAppCommand { get; }
        public MvxCommand GoToRepositoryCommand { get; }
        public MvxCommand GoToTranslationProjectCommand { get; }
        public MvxCommand GoToDesignerTwitterAccountCommand { get; }
        public MvxCommand GoToContributionPageCommand { get; }
        public string Version { get; } = "0.4.5";
        public string Website { get; } = "www.foo.ch";
        public string SupportMail { get; } = "foo@me.ch";
    }
}
