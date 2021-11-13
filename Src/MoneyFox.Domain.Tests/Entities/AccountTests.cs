using FluentAssertions;
using MoneyFox.Domain.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Xunit;

namespace MoneyFox.Domain.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class AccountTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_NameEmpty_ArgumentNullException(string name) =>
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Account(name));

        [Theory]
        [InlineData(-12, true)]
        [InlineData(12, false)]
        [InlineData(0, false)]
        public void Ctor_Balance_IsOverdrawnCorrectSet(decimal currentBalance, bool expectedIsOverdrawn)
        {
            // Arrange

            // Act / Assert
            var account = new Account("test", currentBalance);

            // Assert
            account.IsOverdrawn.Should().Be(expectedIsOverdrawn);
        }

        [Fact]
        public void Ctor_NoParams_DefaultValuesSet()
        {
            // Arrange
            const string testName = "test";

            // Act / Assert
            var account = new Account(testName);

            // Assert
            account.Name.Should().Be(testName);
            account.CurrentBalance.Should().Be(0);
            account.Note.Should().BeEmpty();
            account.IsOverdrawn.Should().BeFalse();
            account.IsExcluded.Should().BeFalse();
            account.ModificationDate.Should().BeAfter(DateTime.Now.AddSeconds(-1));
            account.CreationTime.Should().BeAfter(DateTime.Now.AddSeconds(-1));
        }

        [Fact]
        public void Ctor_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testName = "test";
            const decimal testBalance = 10;
            const string testNote = "foo";
            const bool testExcluded = true;

            // Act / Assert
            var account = new Account(testName, testBalance, testNote, testExcluded);

            // Assert
            account.Name.Should().Be(testName);
            account.CurrentBalance.Should().Be(testBalance);
            account.Note.Should().Be(testNote);
            account.IsExcluded.Should().Be(testExcluded);
            account.IsOverdrawn.Should().BeFalse();
        }

        [Fact]
        public void CtorDeactivatedShouldBeFalse()
        {
            // Arrange
            // Act
            var testAccount = new Account("foo");

            // Assert
            testAccount.IsDeactivated.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void UpdateData_NameEmpty_ArgumentNullException(string name)
        {
            // Arrange
            var testAccount = new Account("test");

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => testAccount.UpdateAccount(name));
        }

        [Theory]
        [InlineData(-12, true)]
        [InlineData(12, false)]
        [InlineData(0, false)]
        public void UpdateData_Balance_IsOverdrawnCorrectSet(decimal currentBalance, bool expectedIsOverdrawn)
        {
            // Arrange
            var testAccount = new Account("test");

            // Act / Assert
            testAccount.UpdateAccount(testAccount.Name, currentBalance);

            // Assert
            testAccount.IsOverdrawn.Should().Be(expectedIsOverdrawn);
        }

        [Fact]
        public void UpdateData_NoParams_DefaultValuesSet()
        {
            // Arrange
            const string testname = "test";
            var testAccount = new Account("foo");

            // Act / Assert
            testAccount.UpdateAccount(testname);

            // Assert
            testAccount.Name.Should().Be(testname);
            testAccount.CurrentBalance.Should().Be(0);
            testAccount.Note.Should().BeEmpty();
            testAccount.IsOverdrawn.Should().BeFalse();
            testAccount.IsExcluded.Should().BeFalse();
        }

        [Fact]
        public void UpdateData_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testname = "test";
            const decimal testBalance = 10;
            const string testnote = "foo";
            const bool testExcluded = true;

            var testAccount = new Account("foo");

            // Act / Assert
            testAccount.UpdateAccount(testname, testBalance, testnote, testExcluded);

            // Assert
            testAccount.Name.Should().Be(testname);
            testAccount.CurrentBalance.Should().Be(testBalance);
            testAccount.Note.Should().Be(testnote);
            testAccount.IsExcluded.Should().Be(testExcluded);
            testAccount.IsOverdrawn.Should().BeFalse();
        }

        [Fact]
        public void UpdateData_Params_ModificationDateSet()
        {
            // Arrange

            var testAccount = new Account("foo");

            // Act / Assert
            testAccount.UpdateAccount("asdf", 123);

            // Assert
            testAccount.ModificationDate.Should().BeAfter(DateTime.Now.AddSeconds(-1));
        }

        [Fact]
        public void AddPaymentAmount_PaymentNull_ArgumentNullException()
        {
            // Arrange
            var account = new Account("test");

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => account.AddPaymentAmount(null));
        }

        [Theory]
        [InlineData(PaymentType.Expense, 50)]
        [InlineData(PaymentType.Income, 150)]
        public void AddPaymentAmount_IncomeExpense_CurrentBalanceAdjustedCorrectly(PaymentType paymentType, decimal expectedBalance)
        {
            // Arrange
            var account = new Account("Test", 100);

            // Act
            // AddPaymentAmount executed in the clear method
            new Payment(DateTime.Today, 50, paymentType, account);

            // Assert
            account.CurrentBalance.Should().Be(expectedBalance);
        }

        [Theory]
        [InlineData(PaymentType.Expense)]
        [InlineData(PaymentType.Income)]
        public void AddPaymentAmount_IncomeExpenseNotCleared_CurrentBalanceNotAdjusted(PaymentType paymentType)
        {
            // Arrange
            var account = new Account("Test", 100);
            var payment = new Payment(DateTime.Today.AddDays(2), 50, paymentType, account);

            // Act
            account.AddPaymentAmount(payment);

            // Assert
            account.CurrentBalance.Should().Be(100);
        }

        [Fact]
        public void AddPaymentAmount_Transfer_CurrentBalanceAdjustedCorrectly()
        {
            // Arrange
            var chargedAccount = new Account("Test", 100);
            var targetAccount = new Account("Test", 100);

            FieldInfo chargedAccountId = typeof(Account).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            chargedAccountId.SetValue(chargedAccount, 3);

            FieldInfo targetAccountId = typeof(Account).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            targetAccountId.SetValue(targetAccount, 4);

            // Act
            // AddPaymentAmount executed in the clear method
            new Payment(DateTime.Today, 50, PaymentType.Transfer, chargedAccount, targetAccount);

            // Assert
            chargedAccount.CurrentBalance.Should().Be(50);
            targetAccount.CurrentBalance.Should().Be(150);
        }

        [Fact]
        public void AddPaymentAmount_Params_ModificationDateSet()
        {
            // Arrange
            var testAccount = new Account("foo");
            var payment = new Payment(DateTime.Today.AddDays(2), 50, PaymentType.Income, testAccount);

            // Act
            testAccount.AddPaymentAmount(payment);

            // Assert
            testAccount.ModificationDate.Should().BeAfter(DateTime.Now.AddSeconds(-1));
        }

        [Fact]
        public void RemovePaymentAmount_PaymentNull_ArgumentNullException()
        {
            // Arrange
            var account = new Account("test");

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => account.RemovePaymentAmount(null));
        }

        [Theory]
        [InlineData(PaymentType.Expense, 100)]
        [InlineData(PaymentType.Income, 100)]
        public void RemovePaymentAmount_IncomeExpense_CurrentBalanceAdjustedCorrectly(PaymentType paymentType, decimal expectedBalance)
        {
            // Arrange
            var account = new Account("Test", 100);
            var payment = new Payment(DateTime.Today, 50, paymentType, account);

            // Act
            account.RemovePaymentAmount(payment);

            // Assert
            account.CurrentBalance.Should().Be(expectedBalance);
        }

        [Fact]
        public void RemovePaymentAmount_Transfer_CurrentBalanceAdjustedCorrectly()
        {
            // Arrange
            var chargedAccount = new Account("Test", 100);
            var targetAccount = new Account("Test", 100);

            FieldInfo chargedAccountId = typeof(Account).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            chargedAccountId.SetValue(chargedAccount, 3);

            FieldInfo targetAccountId = typeof(Account).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            targetAccountId.SetValue(targetAccount, 4);

            var payment = new Payment(DateTime.Today, 50, PaymentType.Transfer, chargedAccount, targetAccount);

            // Act
            chargedAccount.RemovePaymentAmount(payment);
            targetAccount.RemovePaymentAmount(payment);

            // Assert
            chargedAccount.CurrentBalance.Should().Be(100);
            targetAccount.CurrentBalance.Should().Be(100);
        }

        [Fact]
        public void RemovePaymentAmount_Params_ModificationDateSet()
        {
            // Arrange
            var testAccount = new Account("foo");
            var payment = new Payment(DateTime.Today.AddDays(2), 50, PaymentType.Income, testAccount);

            // Act
            testAccount.RemovePaymentAmount(payment);

            // Assert
            testAccount.ModificationDate.Should().BeAfter(DateTime.Now.AddSeconds(-1));
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
}
