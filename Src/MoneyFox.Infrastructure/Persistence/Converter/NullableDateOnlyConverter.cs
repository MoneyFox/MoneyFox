namespace MoneyFox.Infrastructure.Persistence.Converter;

using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

[UsedImplicitly]
internal sealed class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
{
    public NullableDateOnlyConverter() : base(
        convertToProviderExpression: d => d == null ? null : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
        convertFromProviderExpression: d => d == null ? null : new DateOnly?(DateOnly.FromDateTime(d.Value))) { }
}
