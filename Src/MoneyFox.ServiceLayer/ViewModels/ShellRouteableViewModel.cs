using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class ShellRouteableViewModel : RouteableViewModelBase
    {
        public override string UrlPathSegment => "Shell";
        public override IScreen HostScreen { get; }
    }
}
