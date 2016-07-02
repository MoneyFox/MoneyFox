using System;
using System.Globalization;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Visibility;

namespace MoneyFox.Shared.Converter {
    /// <summary>
    ///     Hides the backupdate if it wasn't retrieved so far and shows it again if it is.
    ///     This is to avoid that the DateTime.Min value is displayed.
    /// </summary>
    public class BackupDateVisibilityConverter : MvxBaseVisibilityValueConverter {
        protected override MvxVisibility Convert(object value, object parameter, CultureInfo culture) {
            return (DateTime) value == new DateTime() ? MvxVisibility.Collapsed : MvxVisibility.Visible;
        }
    }
}