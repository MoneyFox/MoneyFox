using MoneyFox.DataAccess;
using MoneyFox.Service.Pocos;
using Xunit;

namespace MoneyFox.Service.IntegrationTests.DataServices
{

    public class AccountServiceTests
    {
        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public AccountServiceTests()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.Migrate();
            }
        }

        [Fact]
        public async void DeleteAccount_RelatedPaymentsRemoved()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var accountRepository = new AccountRepository(factory);
            var paymentRepository = new PaymentRepository(factory);
            var accountService = new AccountService(accountRepository, unitOfWork);

            var account = new AccountEntity
            {
                Name = "Testtext"
            };

            var payment = new PaymentEntity
            {
                Note = "Foo",
                ChargedAccount = account
            };

            accountRepository.Add(account);
            paymentRepository.Add(payment);
            await unitOfWork.Commit();

            Assert.Equal(1, await accountRepository.GetAll().CountAsync());
            Assert.Equal(1, await paymentRepository.GetAll().CountAsync());

            // Act
            await accountService.DeleteAccount(new Account(account));

            // Assert
            Assert.False(await accountRepository.GetAll().AnyAsync());
            Assert.False(await paymentRepository.GetAll().AnyAsync());
        }
    }
}
