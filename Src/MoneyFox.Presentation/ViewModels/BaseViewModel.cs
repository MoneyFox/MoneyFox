using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross.ViewModels;

namespace MoneyFox.Presentation.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, IBaseViewModel
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

    public abstract class BaseViewModel<TParameter> : BaseViewModel, IMvxViewModel<TParameter>
    {
        public abstract void Prepare(TParameter parameter);
    }
}
