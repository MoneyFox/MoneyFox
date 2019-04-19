using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IRoutableViewModel
    {        
        /// <summary>
        ///      Constructor
        /// </summary>
        protected ViewModelBase() {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <summary>
        ///     Provides Access to the LocalizedResources for the current language
        /// </summary>
        public LocalizedResources Resources { get; }

        public abstract string UrlPathSegment { get; }
        public abstract IScreen HostScreen { get; }
    }
}
