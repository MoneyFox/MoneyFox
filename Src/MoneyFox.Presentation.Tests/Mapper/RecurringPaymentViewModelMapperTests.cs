using System;
using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Tests.Collections;
using MoneyFox.Presentation.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Presentation.Tests.Mapper
{
    [ExcludeFromCodeCoverage]
    [Collection("AutoMapperCollection")]
    public class RecurringPaymentViewModelMapperTests
    {
        private readonly IMapper mapper;

        public RecurringPaymentViewModelMapperTests(MapperCollectionFixture fixture)
        {
            mapper = fixture.Mapper;
        }

        [Fact]
        public void RecurringPaymentMappedToCorrectType()
        {
            // Arrange
            var recurringPayment = new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, new Account("asdf"));

            // Act
            var result = mapper.Map<RecurringPaymentViewModel>(recurringPayment);

            // Assert
            result.ShouldBeType<RecurringPaymentViewModel>();
        }

        [Fact]
        public void RecurringPaymentFieldsCorrectlyMapped()
        {
            // Arrange
            var recurringPayment = new RecurringPayment(DateTime.Now, 123, PaymentType.Expense, PaymentRecurrence.Daily, new Account("asdf"), "asdf",
                                                        DateTime.Today, new Account("fasdf"), new Category("gfds"));

            // Act
            var result = mapper.Map<RecurringPaymentViewModel>(recurringPayment);

            // Assert
            result.EndDate.ShouldEqual(recurringPayment.EndDate);
            result.Amount.ShouldEqual(recurringPayment.Amount);
            result.Type.ShouldEqual(recurringPayment.Type);
            result.Recurrence.ShouldEqual(recurringPayment.Recurrence);
            result.Note.ShouldEqual(recurringPayment.Note);
            result.ChargedAccount.Name.ShouldEqual(recurringPayment.ChargedAccount.Name);
            result.Category.Name.ShouldEqual(recurringPayment.Category?.Name);
            result.EndDate.ShouldEqual(recurringPayment.EndDate);
        }
    }
}
