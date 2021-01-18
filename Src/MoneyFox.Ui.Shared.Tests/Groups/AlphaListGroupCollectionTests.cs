using FluentAssertions;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Ui.Shared.ViewModels.Accounts;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Xunit;

namespace MoneyFox.Ui.Shared.Tests.Groups
{
    [ExcludeFromCodeCoverage]
    public class AlphaListGroupCollectionTests
    {
        [Fact]
        public void CreateGroupReturnsCorrectGroup()
        {
            // Arrange
            var accountList = new List<AccountViewModel>
            {
                new AccountViewModel{Name = "a"},
                new AccountViewModel{Name= "b"}
            };

            // Act
            List<AlphaGroupListGroupCollection<AccountViewModel>> createdGroup
                = AlphaGroupListGroupCollection<AccountViewModel>.CreateGroups(accountList,
                                                                         CultureInfo.CurrentUICulture,
                                                                         s => s.Name);
            // Assert
            createdGroup.Should().HaveCount(2);
            createdGroup[0][0].Name.Should().Be("a");
            createdGroup[1][0].Name.Should().Be("b");
        }
    }
}
