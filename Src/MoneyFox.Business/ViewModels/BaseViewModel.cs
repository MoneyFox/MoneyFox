using System.Globalization;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public class BaseViewModel : MvxViewModel
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
