using MoneyFox.Domain.Exceptions;
using MoneyFox.Presentation.ViewModels;
using MoneyFox.Ui.Shared.Groups;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
                                                           () => AlphaGroupListGroupCollection<PaymentViewModel>
                                                                  .CreateGroups(null, CultureInfo.CurrentUICulture, s => string.Empty));
        }
    }
}
