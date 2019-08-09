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
            mapper = fixture.mapper;
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
    }
}
