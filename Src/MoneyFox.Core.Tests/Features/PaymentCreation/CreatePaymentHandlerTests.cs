namespace MoneyFox.Core.Tests.Features.PaymentCreation;

using Core.Common.Extensions;
using Core.Features.PaymentCreation;
using Domain;
using Domain.Tests.TestFramework;
using static Domain.Tests.TestFramework.PaymentAssertion;

public sealed class CreatePaymentHandlerTests : InMemoryTestBase
{
    private readonly CreatePayment.Handler handler;

    public CreatePaymentHandlerTests()
    {
        handler = new(Context);
    }

    [Fact]
    public async Task CreatePayment()
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
            ChargedAccountId: testPayment.ChargedAccount.Id,
            TargetAccountId: null,
            Amount: new(amount: testPayment.Amount, currency: Currencies.CHF),
            Type: testPayment.Type,
            Date: testPayment.Date.ToDateOnly(),
            CategoryId: testPayment.Category?.Id,
            RecurringTransactionId: testPayment.RecurringTransactionId,
            Note: testPayment.Note);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbPayment = Context.Payments.Single();
        AssertPayment(actual: dbPayment, expected: testPayment);
    }

    [Fact]
    public async Task CorrectPaymentAmount_WhenNegative()
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
            ChargedAccountId: testPayment.ChargedAccount.Id,
            TargetAccountId: null,
            Amount: new(amount: -testPayment.Amount, currency: Currencies.CHF),
            Type: testPayment.Type,
            Date: testPayment.Date.ToDateOnly(),
            CategoryId: testPayment.Category?.Id,
            RecurringTransactionId: testPayment.RecurringTransactionId,
            Note: testPayment.Note);

        await handler.Handle(command: command, cancellationToken: CancellationToken.None);

        // Assert
        var dbPayment = Context.Payments.Single();
        AssertPayment(actual: dbPayment, expected: testPayment);
    }
}
