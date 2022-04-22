namespace MoneyFox.Tests.Presentation.Groups
{

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using FluentAssertions;
    using MoneyFox.Groups;
    using MoneyFox.ViewModels.Accounts;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class AlphaListGroupCollectionTests
    {
        [Fact]
        public void CreateGroupReturnsCorrectGroup()
        {
            // Arrange
            var accountList = new List<AccountViewModel> { new AccountViewModel { Name = "a" }, new AccountViewModel { Name = "b" } };

            // Act
            var createdGroup = AlphaGroupListGroupCollection<AccountViewModel>.CreateGroups(
                items: accountList,
                ci: CultureInfo.CurrentUICulture,
                getKey: s => s.Name);

            // Assert
            createdGroup.Should().HaveCount(2);
            createdGroup[0][0].Name.Should().Be("a");
            createdGroup[1][0].Name.Should().Be("b");
        }
    }

}
