namespace MoneyFox.Ui.Tests.Views.Accounts;

using FluentAssertions;
using Ui.Views.Accounts;
using Xunit;

public class AccountViewModelTests
{
    public static IEnumerable<object[]> Data
        => new List<object[]>
        {
            new object[] { new AccountViewModel(), null!, false },
            new object[] { new AccountViewModel(), new AccountViewModel(), true },
            new object[] { new AccountViewModel { Id = 2 }, new AccountViewModel { Id = 3 }, false }
        };

    [Theory]
    [MemberData(nameof(Data))]
    public void EqualsReturnsFalseOnNull(AccountViewModel vm1, AccountViewModel vm2, bool result)
    {
        // Act / Assert
        _ = vm1.Equals(vm2).Should().Be(result);
    }
}
