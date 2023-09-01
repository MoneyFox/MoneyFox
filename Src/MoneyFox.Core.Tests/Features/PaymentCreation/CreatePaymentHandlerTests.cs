namespace MoneyFox.Core.Tests.Features.PaymentCreation;
using System.Linq;
using System.Threading.Tasks;
using MoneyFox.Core.Common.Extensions;
using MoneyFox.Core.Features.PaymentCreation;
using MoneyFox.Domain;
using MoneyFox.Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.PaymentAssertion;

public sealed class CreatePaymentHandlerTests : InMemoryTestBase
{
    private readonly CreatePayment.Handler handler;

    public CreatePaymentHandlerTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task AddRecurringTransactionToDb()
    {
        // Arrange
        var testAccount = new TestData.IncludedAccount().CreateDbAccount();
        Context.Add(testAccount);
        var testCategory = new TestData.CategoryBeverages().CreateDbCategory();
        Context.Add(testCategory);
        await Context.SaveChangesAsync();
        var testPayment = new TestData.UnclearedExpense();

        // Act
        var command = new CreatePayment.Command(
            testPayment.ChargedAccount.Id,
            null,
            new Money(testPayment.Amount, Currencies.CHF),
            testPayment.Type,
            testPayment.Date.ToDateOnly(),
            testPayment.Category?.Id,
            testPayment.RecurringTransactionId,
            testPayment.Note);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbPayment = Context.Payments.Single();
        AssertPayment(dbPayment, testPayment);
    }
}
