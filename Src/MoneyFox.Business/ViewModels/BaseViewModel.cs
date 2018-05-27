using System.Globalization;
using MoneyFox.Business.Helpers;
using MoneyFox.Foundation.Resources;
using MvvmCross.ViewModels;

namespace MoneyFox.Business.ViewModels
{
    public interface IBaseViewModel
    {
        LocalizedResources Resources { get; }
    }

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
