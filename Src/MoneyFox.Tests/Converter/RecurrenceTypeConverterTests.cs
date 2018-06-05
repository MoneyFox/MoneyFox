using System.Collections.Generic;
using MoneyFox.Converter;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using Should;
using Xunit;

namespace MoneyFox.Tests.Converter
{
    public class RecurrenceTypeConverterTests
    {
        public static IEnumerable<object[]> TestRecurrences
        {
            get
            {
                yield return new object[] { PaymentRecurrence.Daily, Strings.DailyLabel };
                yield return new object[] { PaymentRecurrence.DailyWithoutWeekend, Strings.DailyWithoutWeekendLabel };
                yield return new object[] { PaymentRecurrence.Weekly, Strings.WeeklyLabel };
                yield return new object[] { PaymentRecurrence.Biweekly, Strings.BiweeklyLabel };
                yield return new object[] { PaymentRecurrence.Monthly, Strings.MonthlyLabel };
                yield return new object[] { PaymentRecurrence.Bimonthly, Strings.BimonthlyLabel };
                yield return new object[] { PaymentRecurrence.Yearly, Strings.YearlyLabel };
            }
        }

        [Theory]
        [MemberData(nameof(TestRecurrences))]
        public void Convert_CorrectValue(PaymentRecurrence type, string result)
        {
            new RecurrenceTypeConverter().Convert(type, null, null, null).ShouldEqual(result);
        }
    }
}
