using FluentAssertions;
using MoneyFox.Groups;
using MoneyFox.ViewModels.Payments;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace MoneyFox.Tests.Groups
{
    [ExcludeFromCodeCoverage]
    public class DateListGroupCollectionTests
    {
        [Fact]
        public void CreateGroupReturnsCorrectGroup()
        {
            // Arrange
            var paymentList = new List<PaymentViewModel>
            {
                new PaymentViewModel {Id = 1, Date = DateTime.Now},
                new PaymentViewModel {Id = 2, Date = DateTime.Now.AddMonths(-1)}
            };

            // Act
            var createdGroup
                = DateListGroupCollection<PaymentViewModel>.CreateGroups(
                    paymentList,
                    s => s.Date.ToString("D", CultureInfo.CurrentCulture),
                    s => s.Date);
            // Assert
            createdGroup.Should().HaveCount(2);
            createdGroup[0][0].Id.Should().Be(1);
            createdGroup[1][0].Id.Should().Be(2);
        }
    }
}