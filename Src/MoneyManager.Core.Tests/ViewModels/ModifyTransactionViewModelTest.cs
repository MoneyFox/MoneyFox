using System;
using System.Collections;
using System.Collections.Generic;
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
        public static IEnumerable TransactionModTitles
        {
            get
            {
                //Edit Titles
                yield return new object[] { TransactionType.Spending, true, Strings.EditSpendingTitle, };
                yield return new object[] { TransactionType.Income, true, Strings.EditIncomeTitle };
                yield return new object[] { TransactionType.Transfer, true, Strings.EditTransferTitle };

                //Add Titles
                yield return new object[] { TransactionType.Spending, false, Strings.AddSpendingTitle, };
                yield return new object[] { TransactionType.Income, false, Strings.AddIncomeTitle };
                yield return new object[] { TransactionType.Transfer, false, Strings.AddTransferTitle };
            }
        }

        [Theory]
        [MemberData(nameof(TransactionModTitles))]
        public void Title_TransactionTypeDifferentModes_CorrectTitle(TransactionType type, bool isEditMode, string result)
        {
            var accountRepoSetup = new Mock<IDataAccess<Account>>();
            accountRepoSetup.Setup(x => x.LoadList(null)).Returns(new List<Account>());

            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper),
                new RecurringTransactionDataAccess(dbHelper),
                new AccountRepository(accountRepoSetup.Object))
            {
                Selected = new FinancialTransaction {Type = (int) type}
            };

            var transactionManager = new TransactionManager(transactionRepository,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object);

            var defaultManager = new DefaultManager(new Mock<IAccountRepository>().Object,
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
        }

        [Fact]
        public void Init_SpendingNotEditing_PropertiesSetupCorrectly()
        {
            //Setup
            var dbHelper = new Mock<ISqliteConnectionCreator>().Object;
            var transactionRepositorySetup = new Mock<ITransactionRepository>();
            transactionRepositorySetup.SetupProperty(x => x.Selected);

            var transactionManager = new TransactionManager(transactionRepositorySetup.Object,
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object);

            var accountRepoSetup = new Mock<IAccountRepository>();
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
                new Mock<IAccountRepository>().Object,
                new Mock<IDialogService>().Object);

            var accountRepoSetup = new Mock<IAccountRepository>();
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