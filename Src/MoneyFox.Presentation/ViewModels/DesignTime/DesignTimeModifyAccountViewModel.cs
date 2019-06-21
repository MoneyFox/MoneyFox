using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeModifyAccountViewModel
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
        public RelayCommand SaveCommand { get; }

        /// <inheritdoc />
        public RelayCommand DeleteCommand { get; }

        /// <inheritdoc />
        public RelayCommand CancelCommand { get; }
    }
}
