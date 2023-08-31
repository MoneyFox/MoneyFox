namespace MoneyFox.Infrastructure.Persistence.Converter;

using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

[UsedImplicitly]
internal sealed class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
        convertToProviderExpression: d => d.ToDateTime(TimeOnly.MinValue),
        convertFromProviderExpression: d => DateOnly.FromDateTime(d)) { }
}
