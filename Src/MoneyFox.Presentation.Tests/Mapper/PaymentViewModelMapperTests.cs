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
    public class PaymentViewModelMapperTests
    {
        private readonly IMapper mapper;

        public PaymentViewModelMapperTests(MapperCollectionFixture fixture)
        {
            mapper = fixture.mapper;
        }

        //[Fact]
        //public void PaymentMappedToCorrectType()
        //{
        //    // Arrange
        //    var payment = new Payment(DateTime.Now, 123, PaymentType.Expense, new Account("*asdf"));

        //    // Act
        //    var result = mapper.Map<PaymentViewModel>(payment);

        //    // Assert
        //    result.ShouldBeType<PaymentViewModel>();
        //}
    }
}
