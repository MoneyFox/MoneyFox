using System.Globalization;
using GalaSoft.MvvmLight;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels
{
    public abstract class BaseViewModel : ViewModelBase, IBaseViewModel
    {
        /// <summary>
        ///      Constructor
        /// </summary>
        protected BaseViewModel()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <summary>
        ///     Provides Access to the LocalizedResources for the current language
        /// </summary>
        public LocalizedResources Resources { get; }
    }
}
