using MoneyFox.Foundation.Exceptions;
using MoneyFox.Presentation.Groups;
using MoneyFox.Presentation.ViewModels;
using System;
using Xunit;

namespace MoneyFox.Presentation.Tests.Groups
{
    public class DateListGroupCollectionTests
    {
        [Fact]
        public void CreateGroups_Null_ArgumentNullExceptionThrown()
        {
            // Arrange
            // Act / Assert
            Assert.Throws<GroupListParameterNullException>(() => DateListGroupCollection<PaymentViewModel>.CreateGroups(null, s => "", s => DateTime.Now));
        }
    }
}
