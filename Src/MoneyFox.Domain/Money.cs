namespace MoneyFox.Domain;

public record Money
{
    public Money(decimal amount, string currencyAlphaIsoCode)
        : this(amount, Currencies.Get(currencyAlphaIsoCode))
    { }

    public Money(decimal amount, int currencyNumericIsoCode)
        : this(amount, Currencies.Get(currencyNumericIsoCode))
    { }

    public Money(decimal amount, Currency currency)
    {
        Currency = currency;
        Amount = Math.Round(amount, currency.Precision, MidpointRounding.AwayFromZero);
    }

    public decimal Amount { get; }
    public Currency Currency { get; }

    public static Money Zero(string currencyAlphaIsoCode) => new(amount: 0, currencyAlphaIsoCode);

    public static Money Zero(int currencyNumericIsoCode) => new(amount: 0, currencyNumericIsoCode);

    public static Money Zero(Currency currency) => new(amount: 0, currency);
}
