using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using MoneyFox.Foundation;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows.ConverterBindings
{
    public class ListToItemCollectionBinding : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            return fromType == typeof(List<PaymentRecurrence>) && toType == typeof(ItemCollection) ? 100 : 0;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            try
            {
                result = (ItemCollection) from;
                return true;
            } catch (InvalidCastException ex)
            {
                this.Log().Error(ex);
                result = null;
                return false;
            }
        }
    }
}
