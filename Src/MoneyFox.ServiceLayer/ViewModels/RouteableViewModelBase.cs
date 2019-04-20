using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public abstract class RouteableViewModelBase : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {        
        /// <summary>
        ///      Constructor
        /// </summary>
        protected RouteableViewModelBase() {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
            Activator = new ViewModelActivator();
        }

        /// <summary>
        ///     Provides Access to the LocalizedResources for the current language
        /// </summary>
        public LocalizedResources Resources { get; }

        public ViewModelActivator Activator { get; }

        public abstract string UrlPathSegment { get; }
        public abstract IScreen HostScreen { get; }
    }
}
