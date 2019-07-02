using MoneyFox.Foundation.Exceptions;
using MoneyFox.Presentation.Groups;
using MoneyFox.Presentation.ViewModels;
using System;
using System.Globalization;
using Xunit;

namespace MoneyFox.Presentation.Tests.Groups
{
    public class AlphaGroupListGroupCollectionTests
    {
        [Fact]
        public void CreateGroups_Null_ArgumentNullExceptionThrown()
        {
            // Arrange
            // Act / Assert
            Assert.Throws<GroupListParameterNullException>(() => AlphaGroupListGroupCollection<PaymentViewModel>.CreateGroups(null, CultureInfo.CurrentUICulture, s => ""));
        }
    }
}
