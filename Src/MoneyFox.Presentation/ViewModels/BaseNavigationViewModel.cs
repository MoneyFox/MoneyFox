using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IBaseViewModel
    {
        LocalizedResources Resources { get; }
    }

    public abstract class BaseNavigationViewModel : MvxNavigationViewModel, IBaseViewModel
    {
        /// <summary>
        ///      Constructor
        /// </summary>
        protected BaseNavigationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <summary>
        ///     Provides Access to the LocalizedResources for the current language
        /// </summary>
        public LocalizedResources Resources { get; }
    }

    public abstract class BaseNavigationViewModel<TParameter> : BaseNavigationViewModel, IMvxViewModel<TParameter>
    {
        public abstract void Prepare(TParameter parameter);

        protected BaseNavigationViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
        }
    }
}
