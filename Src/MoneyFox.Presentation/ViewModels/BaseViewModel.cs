using System.Globalization;
using GalaSoft.MvvmLight;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Utilities;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IBaseViewModel
    {
        LocalizedResources Resources { get; }
    }

    public abstract class BaseViewModel : ViewModelBase, IBaseViewModel
    {
        /// <summary>
        ///     Constructor
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
