using System;
using System.Globalization;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows.Converter
{
    public class StringToDoubleBindingTypeConverter : IBindingTypeConverter, IEnableLogger
    {
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            return fromType == typeof(string) && toType == typeof(double) ? 100 : 0;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            try
            {
                result = Convert.ToDouble((string) from, CultureInfo.CurrentCulture);
                return true;
            }
            catch (InvalidCastException ex)
            {
                this.Log().Error(ex);
                result = null;
                return false;
            }
        }
    }
}
