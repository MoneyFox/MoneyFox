using MoneyFox.Domain.Exceptions;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Ui.Shared.Groups;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MoneyFox.Presentation.Tests.Groups
{
    [ExcludeFromCodeCoverage]
    public class DateListGroupCollectionTests
    {
        [Fact]
        public void CreateGroups_Null_ArgumentNullExceptionThrown()
        {
            // Arrange
            // Act / Assert
            Assert.Throws<GroupListParameterNullException>(() => DateListGroupCollection<PaymentViewModel>.CreateGroups(null,
                                                                                                                        s => string.Empty,
                                                                                                                        s => DateTime.Now));
        }
    }
}
