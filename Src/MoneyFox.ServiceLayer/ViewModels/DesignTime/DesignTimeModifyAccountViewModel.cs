using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using MvvmCross.Commands;

namespace MoneyFox.ServiceLayer.ViewModels.DesignTime
{
    public class DesignTimeModifyAccountViewModel : IModifyAccountViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        /// <inheritdoc />
        public bool IsEdit { get; }
        
        /// <inheritdoc />
        public string Title { get; }

        /// <inheritdoc />
        public string AmountString { get; }

        /// <inheritdoc />
        public AccountViewModel SelectedAccount { get; }

        /// <inheritdoc />
        public MvxAsyncCommand SaveCommand { get; }

        /// <inheritdoc />
        public MvxAsyncCommand DeleteCommand { get; }

        /// <inheritdoc />
        public MvxAsyncCommand CancelCommand { get; }
    }
}
