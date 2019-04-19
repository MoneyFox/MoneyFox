using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public override string UrlPathSegment => "Shell";
        public override IScreen HostScreen { get; }
    }
}
