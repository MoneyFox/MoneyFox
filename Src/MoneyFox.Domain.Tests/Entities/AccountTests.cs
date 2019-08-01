using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MoneyFox.Domain.Entities;
using Should;
using Xunit;

namespace MoneyFox.Domain.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class AccountTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_NameEmpty_ArgumentNullException(string name)
        {
            // Arrange

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => new Account(name));
        }

        [Theory]
        [InlineData(-12, true)]
        [InlineData(12, false)]
        [InlineData(0, false)]
        public void Ctor_Balance_IsOverdrawnCorrectSet(double currentBalance, bool expectedIsOverdrawn)
        {
            // Arrange

            // Act / Assert
            var account = new Account("test", currentBalance);

            // Assert
            account.IsOverdrawn.ShouldEqual(expectedIsOverdrawn);
        }

        [Fact]
        public void Ctor_NoParams_DefaultValuesSet()
        {
            // Arrange
            const string testName = "test";

            // Act / Assert
            var account = new Account(testName);

            // Assert
            account.Name.ShouldEqual(testName);
            account.CurrentBalance.ShouldEqual(0);
            account.Note.ShouldBeEmpty();
            account.IsOverdrawn.ShouldBeFalse();
            account.IsExcluded.ShouldBeFalse();
            account.CreationTime.ShouldBeInRange(DateTime.Now.AddSeconds(-1), DateTime.Now);
        }

        [Fact]
        public void Ctor_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testName = "test";
            const double testBalance = 10;
            const string testNote = "foo";
            const bool testExcluded = true;

            // Act / Assert
            var account = new Account(testName, testBalance, testNote, testExcluded);

            // Assert
            account.Name.ShouldEqual(testName);
            account.CurrentBalance.ShouldEqual(testBalance);
            account.Note.ShouldEqual(testNote);
            account.IsExcluded.ShouldEqual(testExcluded);
            account.IsOverdrawn.ShouldBeFalse();
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
        public void UpdateData_Balance_IsOverdrawnCorrectSet(double currentBalance, bool expectedIsOverdrawn)
        {
            // Arrange
            var testAccount = new Account("test");

            // Act / Assert
            testAccount.UpdateAccount(testAccount.Name, currentBalance: currentBalance);

            // Assert
            testAccount.IsOverdrawn.ShouldEqual(expectedIsOverdrawn);
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
            testAccount.Name.ShouldEqual(testname);
            testAccount.CurrentBalance.ShouldEqual(0);
            testAccount.Note.ShouldBeEmpty();
            testAccount.IsOverdrawn.ShouldBeFalse();
            testAccount.IsExcluded.ShouldBeFalse();
        }

        [Fact]
        public void UpdateData_Params_ValuesCorrectlySet()
        {
            // Arrange
            const string testname = "test";
            const double testBalance = 10;
            const string testnote = "foo";
            const bool testExcluded = true;

            var testAccount = new Account("foo");

            // Act / Assert
            testAccount.UpdateAccount(testname, testBalance, testnote, testExcluded);

            // Assert
            testAccount.Name.ShouldEqual(testname);
            testAccount.CurrentBalance.ShouldEqual(testBalance);
            testAccount.Note.ShouldEqual(testnote);
            testAccount.IsExcluded.ShouldEqual(testExcluded);
            testAccount.IsOverdrawn.ShouldBeFalse();
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
        public void AddPaymentAmount_IncomeExpense_CurrentBalanceAdjustedCorrectly(PaymentType paymentType, double expectedBalance)
        {
            // Arrange
            var account = new Account("Test", 100);

            // Act
            // AddPaymentAmount executed in the clear method
            new Payment(DateTime.Today, 50, paymentType, account);

            // Assert
            account.CurrentBalance.ShouldEqual(expectedBalance);
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
            account.CurrentBalance.ShouldEqual(100);
        }


        [Fact]
        public void AddPaymentAmount_Transfer_CurrentBalanceAdjustedCorrectly()
        {
            // Arrange
            var chargedAccount = new Account("Test", 100);
            var targetAccount = new Account("Test", 100);

            var chargedAccountId = typeof(Account).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            chargedAccountId.SetValue(chargedAccount, 3);

            var targetAccountId = typeof(Account).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            targetAccountId.SetValue(targetAccount, 4);

            // Act
            // AddPaymentAmount executed in the clear method
            new Payment(DateTime.Today, 50, PaymentType.Transfer, chargedAccount, targetAccount);

            // Assert
            chargedAccount.CurrentBalance.ShouldEqual(50);
            targetAccount.CurrentBalance.ShouldEqual(150);
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
        public void RemovePaymentAmount_IncomeExpense_CurrentBalanceAdjustedCorrectly(PaymentType paymentType, double expectedBalance)
        {
            // Arrange
            var account = new Account("Test", 100);
            var payment = new Payment(DateTime.Today, 50, paymentType, account);

            // Act
            account.RemovePaymentAmount(payment);

            // Assert
            account.CurrentBalance.ShouldEqual(expectedBalance);
        }


        [Fact]
        public void RemovePaymentAmount_Transfer_CurrentBalanceAdjustedCorrectly()
        {
            // Arrange
            var chargedAccount = new Account("Test", 100);
            var targetAccount = new Account("Test", 100);

            var chargedAccountId = typeof(Account).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            chargedAccountId.SetValue(chargedAccount, 3);

            var targetAccountId = typeof(Account).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            targetAccountId.SetValue(targetAccount, 4);


            var payment = new Payment(DateTime.Today, 50, PaymentType.Transfer, chargedAccount, targetAccount);

            // Act
            chargedAccount.RemovePaymentAmount(payment);
            targetAccount.RemovePaymentAmount(payment);

            // Assert
            chargedAccount.CurrentBalance.ShouldEqual(100);
            targetAccount.CurrentBalance.ShouldEqual(100);
        }
    }
}
