namespace MoneyFox.Core.Tests.Domain.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Core.ApplicationCore.Domain.Aggregates.AccountAggregate;
using FluentAssertions;


public class AccountTests
{
    [Fact]
    public void DefaultValuesCorrectlySet()
    {
        // Arrange
        const string testName = "test";

        // Act / Assert
        var account = new Account(testName);

        // Assert
        account.Name.Should().Be(testName);
        account.CurrentBalance.Should().Be(0);
        account.Note.Should().BeEmpty();
        account.IsExcluded.Should().BeFalse();
    }

    [Fact]
    public void ValuesCorrectlySetAfterConstructor()
    {
        // Arrange
        const string testName = "test";
        const decimal testBalance = 10;
        const string testNote = "foo";
        const bool testExcluded = true;

        // Act / Assert
        var account = new Account(name: testName, initialBalance: testBalance, note: testNote, isExcluded: testExcluded);

        // Assert
        account.Name.Should().Be(testName);
        account.CurrentBalance.Should().Be(testBalance);
        account.Note.Should().Be(testNote);
        account.IsExcluded.Should().Be(testExcluded);
    }

    [Fact]
    public void NotDeactivatedOnCreation()
    {
        // Arrange
        // Act
        var testAccount = new Account("foo");

        // Assert
        testAccount.IsDeactivated.Should().BeFalse();
    }

    [Fact]
    public void UpdateData_NameEmpty_ArgumentNullException()
    {
        // Arrange
        var testAccount = new Account("test");

        // Act / Assert
        Assert.Throws<ArgumentException>(() => testAccount.Change(string.Empty));
    }

    [Fact]
    public void UpdateData_NoParams_DefaultValuesSet()
    {
        // Arrange
        const string testName = "test";
        var testAccount = new Account("foo");

        // Act / Assert
        testAccount.Change(testName);

        // Assert
        testAccount.Name.Should().Be(testName);
        testAccount.CurrentBalance.Should().Be(0);
        testAccount.Note.Should().BeEmpty();
        testAccount.IsExcluded.Should().BeFalse();
    }

    [Fact]
    public void UpdateData_Params_ValuesCorrectlySet()
    {
        // Arrange
        const string testname = "test";
        const string testnote = "foo";
        const bool testExcluded = true;
        var testAccount = new Account("foo");

        // Act / Assert
        testAccount.Change(name: testname, note: testnote, isExcluded: testExcluded);

        // Assert
        testAccount.Name.Should().Be(testname);
        testAccount.Note.Should().Be(testnote);
        testAccount.IsExcluded.Should().Be(testExcluded);
    }

    [Theory]
    [InlineData(PaymentType.Expense, 50)]
    [InlineData(PaymentType.Income, 150)]
    public void AddPaymentAmount_IncomeExpense_CurrentBalanceAdjustedCorrectly(PaymentType paymentType, decimal expectedBalance)
    {
        // Arrange
        var account = new Account(name: "Test", initialBalance: 100);

        // Act
        // AddPaymentAmount executed in the clear method
        new Payment(date: DateTime.Today, amount: 50, type: paymentType, chargedAccount: account);

        // Assert
        account.CurrentBalance.Should().Be(expectedBalance);
    }

    [Theory]
    [InlineData(PaymentType.Expense)]
    [InlineData(PaymentType.Income)]
    public void AddPaymentAmount_IncomeExpenseNotCleared_CurrentBalanceNotAdjusted(PaymentType paymentType)
    {
        // Arrange
        var account = new Account(name: "Test", initialBalance: 100);
        var payment = new Payment(date: DateTime.Today.AddDays(2), amount: 50, type: paymentType, chargedAccount: account);

        // Act
        account.AddPaymentAmount(payment);

        // Assert
        account.CurrentBalance.Should().Be(100);
    }

    [Fact]
    public void AddPaymentAmount_Transfer_CurrentBalanceAdjustedCorrectly()
    {
        // Arrange
        var chargedAccount = new Account(name: "Test", initialBalance: 100);
        var targetAccount = new Account(name: "Test", initialBalance: 100);
        var chargedAccountId = typeof(Account).GetField(name: "<Id>k__BackingField", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic);
        chargedAccountId.SetValue(obj: chargedAccount, value: 3);
        var targetAccountId = typeof(Account).GetField(name: "<Id>k__BackingField", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic);
        targetAccountId.SetValue(obj: targetAccount, value: 4);

        // Act
        // AddPaymentAmount executed in the clear method
        new Payment(
            date: DateTime.Today,
            amount: 50,
            type: PaymentType.Transfer,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount);

        // Assert
        chargedAccount.CurrentBalance.Should().Be(50);
        targetAccount.CurrentBalance.Should().Be(150);
    }

    [Theory]
    [InlineData(PaymentType.Expense, 100)]
    [InlineData(PaymentType.Income, 100)]
    public void RemovePaymentAmount_IncomeExpense_CurrentBalanceAdjustedCorrectly(PaymentType paymentType, decimal expectedBalance)
    {
        // Arrange
        var account = new Account(name: "Test", initialBalance: 100);
        var payment = new Payment(date: DateTime.Today, amount: 50, type: paymentType, chargedAccount: account);

        // Act
        account.RemovePaymentAmount(payment);

        // Assert
        account.CurrentBalance.Should().Be(expectedBalance);
    }

    [Fact]
    public void RemovePaymentAmount_Transfer_CurrentBalanceAdjustedCorrectly()
    {
        // Arrange
        var chargedAccount = new Account(name: "Test", initialBalance: 100);
        var targetAccount = new Account(name: "Test", initialBalance: 100);
        var chargedAccountId = typeof(Account).GetField(name: "<Id>k__BackingField", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic);
        chargedAccountId.SetValue(obj: chargedAccount, value: 3);
        var targetAccountId = typeof(Account).GetField(name: "<Id>k__BackingField", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic);
        targetAccountId.SetValue(obj: targetAccount, value: 4);
        var payment = new Payment(
            date: DateTime.Today,
            amount: 50,
            type: PaymentType.Transfer,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount);

        // Act
        chargedAccount.RemovePaymentAmount(payment);
        targetAccount.RemovePaymentAmount(payment);

        // Assert
        chargedAccount.CurrentBalance.Should().Be(100);
        targetAccount.CurrentBalance.Should().Be(100);
    }

    [Theory]
    [InlineData(PaymentType.Expense)]
    [InlineData(PaymentType.Income)]
    public void ChangePaymentType_TransferToOther_CurrentBalanceAdjustedCorrectly(PaymentType paymentType)
    {
        // Arrange
        var chargedAccount = new Account(name: "Test", initialBalance: 100);
        var targetAccount = new Account(name: "Test", initialBalance: 100);
        var chargedAccountId = typeof(Account).GetField(name: "<Id>k__BackingField", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic);
        chargedAccountId.SetValue(obj: chargedAccount, value: 3);
        var targetAccountId = typeof(Account).GetField(name: "<Id>k__BackingField", bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic);
        targetAccountId.SetValue(obj: targetAccount, value: 4);
        var payment = new Payment(
            date: DateTime.Today,
            amount: 50,
            type: PaymentType.Transfer,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount);

        // Assert (Transfer)
        chargedAccount.CurrentBalance.Should().Be(50);
        targetAccount.CurrentBalance.Should().Be(150);

        // Change from Transfer to Expense/Income
        payment.UpdatePayment(
            date: DateTime.Today,
            amount: 50,
            type: paymentType,
            chargedAccount: chargedAccount,
            targetAccount: targetAccount);

        // Assert (Expense/Income)
        chargedAccount.CurrentBalance.Should().Be(paymentType == PaymentType.Expense ? 50 : 150);
        targetAccount.CurrentBalance.Should().Be(100);

        // Act
        chargedAccount.RemovePaymentAmount(payment);

        // Assert
        chargedAccount.CurrentBalance.Should().Be(100);
    }

    [Fact]
    public void DisableAccountOnDeactivate()
    {
        // Arrange
        var testAccount = new Account("foo");

        // Act
        testAccount.Deactivate();

        // Assert
        testAccount.IsDeactivated.Should().BeTrue();
    }
}
