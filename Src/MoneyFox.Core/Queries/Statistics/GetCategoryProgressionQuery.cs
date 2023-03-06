﻿namespace MoneyFox.Core.Queries.Statistics;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Extensions.QueryObjects;
using Common.Interfaces;
using Common.Settings;
using Domain.Aggregates.AccountAggregate;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetCategoryProgressionQuery : IRequest<IImmutableList<StatisticEntry>>
{
    public GetCategoryProgressionQuery(int categoryId, DateTime startDate, DateTime endDate)
    {
        CategoryId = categoryId;
        StartDate = startDate;
        EndDate = endDate;
        if (startDate > EndDate)
        {
            throw new StartAfterEnddateException();
        }
    }

    public int CategoryId { get; }

    public DateTime StartDate { get; }

    public DateTime EndDate { get; }
}

public class GetCategoryProgressionHandler : IRequestHandler<GetCategoryProgressionQuery, IImmutableList<StatisticEntry>>
{
    private const string RED_HEX_CODE = "#cd3700";
    private const string BLUE_HEX_CODE = "#87cefa";

    private readonly IAppDbContext appDbContext;
    private readonly ISettingsFacade settingsFacade;

    public GetCategoryProgressionHandler(IAppDbContext appDbContext, ISettingsFacade settingsFacade)
    {
        this.appDbContext = appDbContext;
        this.settingsFacade = settingsFacade;
    }

    public async Task<IImmutableList<StatisticEntry>> Handle(GetCategoryProgressionQuery request, CancellationToken cancellationToken)
    {
        var payments = await appDbContext.Payments.Include(x => x.Category)
            .Include(x => x.ChargedAccount)
            .HasCategoryId(categoryId: request.CategoryId)
            .HasDateLargerEqualsThan(request.StartDate.Date)
            .HasDateSmallerEqualsThan(request.EndDate.Date)
            .ToListAsync(cancellationToken);

        List<StatisticEntry> statisticList = new();
        foreach (var group in payments.GroupBy(x => new { x.Date.Month, x.Date.Year }))
        {
            StatisticEntry statisticEntry = new(
                group.Sum(x => GetPaymentAmountForSum(payment: x, request: request)),
                label: $"{group.Key.Month:d2} {group.Key.Year:d4}");

            statisticEntry.ValueLabel = statisticEntry.Value.FormatCurrency(settingsFacade.DefaultCurrency);
            statisticEntry.Color = statisticEntry.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;
            statisticList.Add(statisticEntry);
        }

        return FillReturnList(request: request, statisticEntries: statisticList);
    }

    private IImmutableList<StatisticEntry> FillReturnList(GetCategoryProgressionQuery request, ICollection<StatisticEntry> statisticEntries)
    {
        List<StatisticEntry> returnList = new();
        var startDate = request.StartDate;
        var endDate = request.EndDate.AddMonths(1);
        do
        {
            var items = statisticEntries.Where(x => x.Label == $"{startDate.Month:d2} {startDate.Year:d4}").ToList();
            returnList.AddRange(items);
            if (!items.Any())
            {
                StatisticEntry placeholderItem = new(0, label: $"{startDate.Month:d2} {startDate.Year:d4}");
                placeholderItem.ValueLabel = placeholderItem.Value.FormatCurrency(settingsFacade.DefaultCurrency);
                placeholderItem.Color = placeholderItem.Value >= 0 ? BLUE_HEX_CODE : RED_HEX_CODE;
                returnList.Add(placeholderItem);
            }

            startDate = startDate.AddMonths(1);
        }
        while (startDate.Month != endDate.Month || startDate.Year != endDate.Year);

        return returnList.ToImmutableList();
    }

    private static decimal GetPaymentAmountForSum(Payment payment, GetCategoryProgressionQuery request)
    {
        return payment.Type switch
        {
            PaymentType.Expense => -payment.Amount,
            PaymentType.Income => payment.Amount,
            PaymentType.Transfer => payment.ChargedAccount.Id == request.CategoryId ? -payment.Amount : payment.Amount,
            _ => 0
        };
    }
}
