using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Cirrious.MvvmCross.Test.Core;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.Localization;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class ModifyTransactionViewModelTest : MvxIoCSupportingTest
    {
        [Theory]
        //Edit Titles
        [InlineData(TransactionType.Spending, true, "Edit Spending", "en-US")]
        [InlineData(TransactionType.Income, true, "Edit Income", "en-US")]
        [InlineData(TransactionType.Transfer, true, "Edit Transfer", "en-US")]
        [InlineData(TransactionType.Spending, true, "Ausgabe bearbeiten", "de-CH")]
        [InlineData(TransactionType.Income, true, "Einkommen bearbeiten", "de-CH")]
        [InlineData(TransactionType.Transfer, true, "Überweisung bearbeiten", "de-CH")]
        //Add Titles
        [InlineData(TransactionType.Spending, false, "Add Spending", "en-US")]
        [InlineData(TransactionType.Income, false, "Add Income", "en-US")]
        [InlineData(TransactionType.Transfer, false, "Add Transfer", "en-US")]
        [InlineData(TransactionType.Spending, false, "Ausgabe hinzufügen", "de-CH")]
        [InlineData(TransactionType.Income, false, "Einkommen hinzufügen", "de-CH")]
        [InlineData(TransactionType.Transfer, false, "Überweisung hinzufügen", "de-CH")]
        public void Title_TransactionTypeDifferentModes_CorrectTitle(TransactionType type, bool isEditMode,
            string result, string culture)
        {
            //Setup
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            Strings.Culture = new CultureInfo(culture);

            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper),
                new RecurringTransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) type}
            };

            var transactionManager = new TransactionManager(transactionRepository,
                new Mock<IRepository<Account>>().Object,
                new Mock<IDialogService>().Object);

            var defaultManager = new DefaultManager(new Mock<IRepository<Account>>().Object,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object,
                transactionManager,
                defaultManager)
            {
                IsEdit = isEditMode,
                IsTransfer = true
            };

            //Execute and assert
            viewModel.Title.ShouldBe(result);

            // Reset culture to current culture
            Strings.Culture = CultureInfo.CurrentCulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
        }

        [Fact]
        public void Init_SpendingNotEditing_PropertiesSetupCorrectly()
        {
            //Setup
            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var transactionRepositorySetup = new Mock<ITransactionRepository>();
            transactionRepositorySetup.SetupProperty(x => x.Selected);

            var transactionManager = new TransactionManager(transactionRepositorySetup.Object,
                new Mock<IRepository<Account>>().Object,
                new Mock<IDialogService>().Object);

            var accountRepoSetup = new Mock<IRepository<Account>>();
            accountRepoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var defaultManager = new DefaultManager(accountRepoSetup.Object,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewmodel = new ModifyTransactionViewModel(transactionRepositorySetup.Object,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object,
                transactionManager,
                defaultManager);

            //Execute and Assert
            viewmodel.SelectedTransaction.ShouldBeNull();

            viewmodel.Init(false, "Spending");
            viewmodel.SelectedTransaction.Type.ShouldBe((int) TransactionType.Spending);
            viewmodel.SelectedTransaction.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedTransaction.IsRecurring.ShouldBeFalse();
        }

        [Fact]
        public void Init_IncomeEditing_PropertiesSetupCorrectly()
        {
            //Setup
            var testEndDate = new DateTime(2099, 1, 31);

            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var transactionRepositorySetup = new Mock<ITransactionRepository>();
            transactionRepositorySetup.SetupGet(x => x.Selected).Returns(new FinancialTransaction
            {
                Type = (int) TransactionType.Income,
                IsRecurring = true,
                RecurringTransaction = new RecurringTransaction
                {
                    EndDate = testEndDate
                }
            });

            var transactionManager = new TransactionManager(transactionRepositorySetup.Object,
                new Mock<IRepository<Account>>().Object,
                new Mock<IDialogService>().Object);

            var accountRepoSetup = new Mock<IRepository<Account>>();
            accountRepoSetup.SetupGet(x => x.Data).Returns(new ObservableCollection<Account>());

            var defaultManager = new DefaultManager(accountRepoSetup.Object,
                new SettingDataAccess(new Mock<IRoamingSettings>().Object));

            var viewmodel = new ModifyTransactionViewModel(transactionRepositorySetup.Object,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object,
                transactionManager,
                defaultManager);

            //Execute and Assert
            viewmodel.SelectedTransaction.ShouldNotBeNull();

            viewmodel.Init(true, "Income");
            viewmodel.SelectedTransaction.Type.ShouldBe((int) TransactionType.Income);
            viewmodel.SelectedTransaction.IsTransfer.ShouldBeFalse();
            viewmodel.SelectedTransaction.IsRecurring.ShouldBeTrue();
            viewmodel.SelectedTransaction.RecurringTransaction.EndDate.ShouldBe(testEndDate);
            viewmodel.SelectedTransaction.RecurringTransaction.IsEndless.ShouldBeFalse();
        }
    }
}