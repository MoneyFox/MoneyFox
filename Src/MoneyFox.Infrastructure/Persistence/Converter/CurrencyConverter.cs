namespace MoneyFox.Infrastructure.Persistence.Converter;

using Domain;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

[UsedImplicitly]
internal sealed class CurrencyConverter : ValueConverter<Currency, string>
{
    public CurrencyConverter() : base(convertToProviderExpression: v => v.AlphaIsoCode, convertFromProviderExpression: v => Currencies.Get(v)) { }
}
