using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Domain;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Converter;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.Converter
{
    [ExcludeFromCodeCoverage]
    public class RecurrenceTypeConverterLogicTests
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
            RecurrenceTypeConverterLogic.GetStringForPaymentRecurrence(type).ShouldEqual(result);
        }
    }
}
