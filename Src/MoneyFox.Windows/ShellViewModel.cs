using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Windows.Views;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows
{
    public class ShellViewModel : ViewModelBase, IScreen
    {
        public ShellViewModel()
        {
            Router = new RoutingState();
            RegisterParts(Locator.CurrentMutable);

            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

            Router.Navigate.Execute(new AccountListViewModel(this));
        }

        public RoutingState Router { get; }

        public override string UrlPathSegment => "Shell";

        public override IScreen HostScreen => this;

        /// <summary>
        ///     Provides Access to the LocalizedResources for the current language
        /// </summary>
        public LocalizedResources Resources { get; }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver) {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            dependencyResolver.RegisterViewsForViewModels(typeof(AccountListView).Assembly);
        }
    }
}
