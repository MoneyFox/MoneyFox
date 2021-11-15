﻿using MoneyFox.Uwp.ConverterLogic;
using MoneyFox.Uwp.ViewModels.Payments;
using System;
using Windows.UI.Xaml.Data;

#nullable enable
namespace MoneyFox.Uwp.Converter
{
    public class PaymentAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var payment = (PaymentViewModel)value;

            if(payment == null)
            {
                return string.Empty;
            }

            return PaymentAmountConverterLogic.GetAmountSign(payment);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotSupportedException();
    }
}