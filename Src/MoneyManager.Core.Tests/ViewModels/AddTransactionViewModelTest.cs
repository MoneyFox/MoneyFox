using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.ViewModels
{
    public class AddTransactionViewModelTest
    {
        [Theory]
        [InlineData(TransactionType.Spending, "Edit Spending")]
        [InlineData(TransactionType.Income, "Edit Income")]
        [InlineData(TransactionType.Transfer, "Edit Transfer")]
        public void Title_EditTransactionType_CorrectTitle(TransactionType type, string result)
        {
            var dbHelper = new Mock<IDbHelper>().Object;
            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int) type}
            };

            var transactionManager = new TransactionManager(transactionRepository,
                new Mock<IRepository<Account>>().Object,
                new Mock<IRepository<RecurringTransaction>>().Object,
                new SettingDataAccess());

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object,
                transactionManager)
            {
                IsEdit = true,
                IsTransfer = true
            };

            viewModel.Title.ShouldBe(result);
        }

        [Theory]
        [InlineData(TransactionType.Spending, "Add Spending")]
        [InlineData(TransactionType.Income, "Add Income")]
        [InlineData(TransactionType.Transfer, "Add Transfer")]
        public void Title_AddTransactionType_CorrectTitle(TransactionType type, string result)
        {
            var dbHelper = new Mock<IDbHelper>().Object;

            var transactionRepository = new TransactionRepository(new TransactionDataAccess(dbHelper))
            {
                Selected = new FinancialTransaction {Type = (int)type }
            };

            var transactionManager = new TransactionManager(transactionRepository,
                new Mock<IRepository<Account>>().Object,
                new Mock<IRepository<RecurringTransaction>>().Object,
                new SettingDataAccess());

            var viewModel = new ModifyTransactionViewModel(transactionRepository,
                new AccountRepository(new AccountDataAccess(dbHelper)),
                new Mock<IDialogService>().Object,
                transactionManager)
            {IsEdit = false};

            viewModel.Title.ShouldBe(result);
        }
    }
}