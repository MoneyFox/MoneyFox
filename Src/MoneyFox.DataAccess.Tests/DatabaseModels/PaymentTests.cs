using Autofac;
using AutoMapper;
using MoneyFox.DataAccess.DatabaseModels;
using MoneyFox.Foundation;
using MoneyFox.Foundation.DataModels;
using Xunit;
using XunitShouldExtension;

namespace MoneyFox.DataAccess.Tests.DatabaseModels
{
    public class PaymentTests
    {
        [Theory]
        [InlineData((int)PaymentType.Income, PaymentType.Income)]
        [InlineData((int)PaymentType.Expense, PaymentType.Expense)]
        [InlineData((int)PaymentType.Transfer, PaymentType.Transfer)]
        public void Enum_CorrectlyParsed(int enumInt, PaymentType type)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DataAccessModule>();
            cb.Build();

            var payment = new Payment
            {
                Id = 9,
                Type = enumInt
            };

            var mappedPayment = Mapper.Map<PaymentViewModel>(payment);
            mappedPayment.Type.ShouldBe(type);
        }

        [Theory]
        [InlineData(PaymentType.Income, (int)PaymentType.Income)]
        [InlineData(PaymentType.Expense, (int)PaymentType.Expense)]
        [InlineData(PaymentType.Transfer, (int)PaymentType.Transfer)]
        public void Enum_CorrectlyParsed(PaymentType type, int enumInt)
        {
            var cb = new ContainerBuilder();
            cb.RegisterModule<DataAccessModule>();
            cb.Build();

            var paymentViewModel = new PaymentViewModel()
            {
                Id = 9,
                Type = type
            };

            var mappedPayment = Mapper.Map<Payment>(paymentViewModel);
            mappedPayment.Type.ShouldBe(enumInt);
        }
    }
}
