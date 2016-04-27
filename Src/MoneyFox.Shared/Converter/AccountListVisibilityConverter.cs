using MoneyFox.Shared.Model;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;

namespace MoneyFox.Windows.Converter
{
    public class ListVisibilityConverter : IMvxValueConverter
    {
        private const string ACCOUNT_PARAMETER = "account";
        private const string PAYMENT_PARAMETER = "payment";
        private const string CATEGORY_PARAMETER = "category";

        /// <summary>
        ///     With this converter is decided if a note is shown in the UI that no element
        ///     is in the list or if this hint should be hidden (because there are elements to show)
        /// </summary>
        /// <param name="value">
        ///     The collection of items to check if empty or not. Expected is 
        ///     something who can be implicitly parsed into an <see cref="ObservableCollection{T}"/> 
        /// </param>
        /// <param name="targetType">Filled by the system, isn't used.</param>
        /// <param name="parameter">
        ///     Possible parameter are "account", "payment" and "category". This indicates what
        ///     Type are in the collection and is needed to parse the value.
        ///     When something else is passed, the return value will always be visible.
        /// </param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null || parameter == null)
            {
                return MvxVisibility.Visible;
            }
                      
            if(parameter.ToString().ToLower() == ACCOUNT_PARAMETER)
            {
                return !((ObservableCollection<Account>)value).Any() ? MvxVisibility.Visible : MvxVisibility.Collapsed;
            }
            else if (parameter.ToString().ToLower() == PAYMENT_PARAMETER)
            {
                return !((ObservableCollection<Payment>)value).Any() ? MvxVisibility.Visible : MvxVisibility.Collapsed;
            }
            else if (parameter.ToString().ToLower() == CATEGORY_PARAMETER)
            {
                return !((ObservableCollection<Category>)value).Any() ? MvxVisibility.Visible : MvxVisibility.Collapsed;
            } 
            else
            {
                return MvxVisibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;        
    }
}
