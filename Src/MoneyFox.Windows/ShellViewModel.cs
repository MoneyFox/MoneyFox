using Autofac;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Windows.Views;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace MoneyFox.Windows
{
    public class ShellViewModel : RouteableViewModelBase, IScreen
    {
        public ShellViewModel()
        {
            Router = new RoutingState();
            RegisterParts();

            Router.Navigate.Execute(new AccountListViewModel(this));
        }

        public RoutingState Router { get; }

        public override string UrlPathSegment => "Shell";

        public override IScreen HostScreen => this;

        private void RegisterParts() {

            var builder = new ContainerBuilder();
            builder.Register((c) => this).As<IScreen>().SingleInstance();
            builder.RegisterModule<WindowsModule>();
            builder.UseAutofacDependencyResolver();

            Locator.CurrentMutable.RegisterViewsForViewModels(typeof(AccountListView).Assembly);
        }
    }
}
