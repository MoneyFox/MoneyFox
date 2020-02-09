using System;
using System.Diagnostics.CodeAnalysis;
using MoneyFox.Domain;
using MoneyFox.Uwp.ViewModels;
using Should;
using Xunit;

namespace MoneyFox.Uwp.Tests.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class RecurringPaymentViewModelTests
    {
        [Fact]
        public void Ctor_SetDefaults()
        {
            // Arrange

            // Act
            var vm = new RecurringPaymentViewModel();

            // Assert
            vm.Recurrence.ShouldEqual(PaymentRecurrence.Daily);
            vm.EndDate?.Date.ShouldEqual(DateTime.Today);
        }
    }
}
