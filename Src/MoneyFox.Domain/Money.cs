namespace MoneyFox.Domain;

public record Money
{
    public Money(decimal amount, string currencyAlphaIsoCode) : this(amount: amount, currency: Currencies.Get(currencyAlphaIsoCode)) { }

    public Money(decimal amount, int currencyNumericIsoCode) : this(amount: amount, currency: Currencies.Get(currencyNumericIsoCode)) { }

    public Money(decimal amount, Currency currency)
    {
        Currency = currency;
        Amount = Math.Round(d: amount, decimals: currency.Precision, mode: MidpointRounding.AwayFromZero);
    }

    public decimal Amount { get; }
    public Currency Currency { get; }

    public static Money Zero(string currencyAlphaIsoCode)
    {
        return new(amount: 0, currencyAlphaIsoCode: currencyAlphaIsoCode);
    }

    public static Money Zero(int currencyNumericIsoCode)
    {
        return new(amount: 0, currencyNumericIsoCode: currencyNumericIsoCode);
    }

    public static Money Zero(Currency currency)
    {
        return new(amount: 0, currency: currency);
    }
}
