namespace MoneyFox.Ui.Tests.Groups;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Common.Groups;
using FluentAssertions;
using MoneyFox.Ui.ViewModels.Accounts;
using Xunit;

[ExcludeFromCodeCoverage]
public class AlphaListGroupCollectionTests
{
    [Fact]
    public void CreateGroupReturnsCorrectGroup()
    {
        // Arrange
        List<AccountViewModel> accountList = new()
        { new AccountViewModel { Name = "a" }, new AccountViewModel { Name = "b" } };

        // Act
        List<AlphaGroupListGroupCollection<AccountViewModel>> createdGroup = AlphaGroupListGroupCollection<AccountViewModel>.CreateGroups(
            items: accountList,
            ci: CultureInfo.CurrentUICulture,
            getKey: s => s.Name);

        // Assert
        _ = createdGroup.Should().HaveCount(2);
        createdGroup[0][0].Name.Should().Be("a");
        createdGroup[1][0].Name.Should().Be("b");
    }
}

