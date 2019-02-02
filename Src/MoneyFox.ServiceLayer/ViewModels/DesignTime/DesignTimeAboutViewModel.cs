using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimeAboutViewModel : IAboutViewModel
    {
        public DesignTimeAboutViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <inheritdoc />
        public LocalizedResources Resources { get; }
        public MvxAsyncCommand GoToWebsiteCommand { get; }
        public MvxAsyncCommand SendMailCommand { get; }
        public MvxCommand RateAppCommand { get; }
        public MvxAsyncCommand GoToRepositoryCommand { get; }
        public MvxAsyncCommand GoToTranslationProjectCommand { get; }
        public MvxAsyncCommand GoToDesignerTwitterAccountCommand { get; }
        public MvxAsyncCommand GoToContributionPageCommand { get; }
        public string Version { get; } = "0.4.5";
        public string Website { get; } = "www.foo.ch";
        public string SupportMail { get; } = "foo@me.ch";
    }
}
