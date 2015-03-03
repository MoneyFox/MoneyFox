#region

using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

#endregion

namespace MoneyManager.Converter {
	public class AmountConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			var numberString = value.ToString().Replace(".", ",");

			return System.Convert.ToDouble(numberString, new CultureInfo("de-CH").NumberFormat).ToString("F2");
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) {
			return value;
		}
	}
}