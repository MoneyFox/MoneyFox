using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MoneyFox.Domain.Exceptions;
using MoneyFox.Presentation.Groups;
using MoneyFox.Presentation.ViewModels;
using Xunit;

namespace MoneyFox.Presentation.Tests.Groups
{
    [ExcludeFromCodeCoverage]
    public class AlphaGroupListGroupCollectionTests
    {
        [Fact]
        public void CreateGroups_Null_ArgumentNullExceptionThrown()
        {
            // Arrange
            // Act / Assert
            Assert.Throws<GroupListParameterNullException>(
                                                           () =>
                                                               AlphaGroupListGroupCollection<PaymentViewModel>
                                                                  .CreateGroups(null, CultureInfo.CurrentUICulture, s => ""));
        }
    }
}
