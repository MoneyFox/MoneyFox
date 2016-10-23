using Autofac;
using AutoMapper;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.DataAccess.Tests.DatabaseModels
{
    public class RecurringPaymentTests
    {
        [Theory]
        [InlineData((int)PaymentType.Income, PaymentType.Income)]
        [InlineData((int)PaymentType.Expense, PaymentType.Expense)]
        [InlineData((int)PaymentType.Transfer, PaymentType.Transfer)]
        public void TypeIntToEnum_CorrectlyParsed(int enumInt, PaymentType type)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DataAccessModule>();
            cb.Build();

            var payment = new RecurringPayment()
            {
                Id = 9,
                Type = enumInt
            };

            var mappedPayment = Mapper.Map<RecurringPaymentViewModel>(payment);
            mappedPayment.Type.ShouldBe(type);
        }

        [Theory]
        [InlineData(PaymentType.Income, (int)PaymentType.Income)]
        [InlineData(PaymentType.Expense, (int)PaymentType.Expense)]
        [InlineData(PaymentType.Transfer, (int)PaymentType.Transfer)]
        public void EnumToTypeInt_CorrectlyParsed(PaymentType type, int enumInt)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DataAccessModule>();
            cb.Build();

            var paymentViewModel = new RecurringPaymentViewModel()
            {
                Id = 9,
                Type = type
            };

            var mappedPayment = Mapper.Map<RecurringPayment>(paymentViewModel);
            mappedPayment.Type.ShouldBe(enumInt);
        }

        [Theory]
        [InlineData((int)PaymentRecurrence.Daily, PaymentRecurrence.Daily)]
        [InlineData((int)PaymentRecurrence.DailyWithoutWeekend, PaymentRecurrence.DailyWithoutWeekend)]
        [InlineData((int)PaymentRecurrence.Weekly, PaymentRecurrence.Weekly)]
        [InlineData((int)PaymentRecurrence.Biweekly, PaymentRecurrence.Biweekly)]
        [InlineData((int)PaymentRecurrence.Monthly, PaymentRecurrence.Monthly)]
        [InlineData((int)PaymentRecurrence.Yearly, PaymentRecurrence.Yearly)]
        public void RecurrenceIntToEnum_CorrectlyParsed(int enumInt, PaymentRecurrence type)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DataAccessModule>();
            cb.Build();

            var payment = new RecurringPayment()
            {
                Id = 9,
                Recurrence = enumInt
            };

            var mappedPayment = Mapper.Map<RecurringPaymentViewModel>(payment);
            mappedPayment.Recurrence.ShouldBe(type);
        }

        [Theory]
        [InlineData(PaymentRecurrence.Daily, (int)PaymentRecurrence.Daily)]
        [InlineData(PaymentRecurrence.DailyWithoutWeekend, (int)PaymentRecurrence.DailyWithoutWeekend)]
        [InlineData(PaymentRecurrence.Weekly, (int)PaymentRecurrence.Weekly)]
        [InlineData(PaymentRecurrence.Biweekly, (int)PaymentRecurrence.Biweekly)]
        [InlineData(PaymentRecurrence.Monthly, (int)PaymentRecurrence.Monthly)]
        [InlineData(PaymentRecurrence.Yearly, (int)PaymentRecurrence.Yearly)]
        public void EnumToRecurrenceInt_CorrectlyParsed(PaymentRecurrence type, int enumInt)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DataAccessModule>();
            cb.Build();

            var paymentViewModel = new RecurringPaymentViewModel
            {
                Id = 9,
                Recurrence = type
            };

            var mappedPayment = Mapper.Map<RecurringPayment>(paymentViewModel);
            mappedPayment.Recurrence.ShouldBe(enumInt);
        }
    }
}
