using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows
{
    public class Bootstrapper : ReactiveObject, IScreen
    {
        public Bootstrapper() {
            Router = new RoutingState();
            RegisterParts(Locator.CurrentMutable);

            Router.Navigate.Execute(new AccountListViewModel(this));
        }

        public RoutingState Router { get; private set; }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver) {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            dependencyResolver.RegisterViewsForViewModels(typeof(AccountListViewModel).Assembly);
        }
    }
}
