using AutoMapper;
using MoneyFox.Domain;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Tests.Collections;
using MoneyFox.Presentation.ViewModels;
using Should;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Presentation.Tests.Mappings
{
    [ExcludeFromCodeCoverage]
    [Collection("AutoMapperCollection")]
    public class PaymentMappingTests
    {
        private readonly IMapper mapper;

        public PaymentMappingTests(MapperCollectionFixture fixture)
        {
            mapper = fixture.Mapper;
        }

        [Fact]
        public void MapToViewModel()
        {
            // Arrange
            var payment = new Payment(DateTime.Today,
                                      124,
                                      PaymentType.Income,
                                      new Account("addd"),
                                      new Account("fasdf"),
                                      new Category("asdf"),
                                      "note");

            // Act
            var result = mapper.Map<PaymentViewModel>(payment);

            // Assert
            result.Date.ShouldEqual(payment.Date);
            result.Amount.ShouldEqual(payment.Amount);
            result.Type.ShouldEqual(payment.Type);
            result.Note.ShouldEqual(payment.Note);
            result.IsRecurring.ShouldEqual(payment.IsRecurring);
            result.Category.Name.ShouldEqual(payment.Category.Name);
        }
    }
}
