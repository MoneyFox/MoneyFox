using Autofac;
using MoneyFox.ServiceLayer.ViewModels;
using MoneyFox.Windows.Views;
using ReactiveUI;
using ReactiveUI.Autofac;
using Splat;
using Splat.Autofac;

namespace MoneyFox.Windows
{
    public class ShellViewModel : ViewModelBase, IScreen
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
            builder.RegisterModule<WindowsModule>();
            builder.UseAutofacDependencyResolver();

            Locator.CurrentMutable.RegisterViewsForViewModels(typeof(AccountListView).Assembly);
        }
    }
}
