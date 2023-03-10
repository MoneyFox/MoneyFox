namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using System.Collections.ObjectModel;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using MediatR;
using Resources.Strings;

internal sealed class PaymentForCategoryListViewModel : BasePageViewModel, IRecipient<PaymentsForCategoryMessage>
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    private ReadOnlyObservableCollection<PaymentDayGroup> paymentDayGroups = null!;

    private string title = string.Empty;

    public PaymentForCategoryListViewModel(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public string Title
    {
        get => title;
        set => SetProperty(field: ref title, newValue: value);
    }

    public ReadOnlyObservableCollection<PaymentDayGroup> PaymentDayGroups
    {
        get => paymentDayGroups;
        private set => SetProperty(field: ref paymentDayGroups, newValue: value);
    }

    public AsyncRelayCommand<PaymentListItemViewModel> GoToEditPaymentCommand
        => new(async pvm => await Shell.Current.GoToAsync($"{Routes.EditPaymentRoute}?paymentId={pvm!.Id}"));

    public void Receive(PaymentsForCategoryMessage message)
    {
        if (message.CategoryId.HasValue)
        {
            var category = mediator.Send(new GetCategoryByIdQuery(message.CategoryId.Value)).GetAwaiter().GetResult();
            Title = string.Format(format: Translations.PaymentsForCategoryTitle, arg0: category.Name);
        }
        else
        {
            Title = Translations.NoCategoryTitle;
        }

        var paymentVms = mapper.Map<List<PaymentListItemViewModel>>(
            mediator.Send(
                    new GetPaymentsForCategorySummary.Query(CategoryId: message.CategoryId, DateRangeFrom: message.StartDate, DateRangeTo: message.EndDate))
                .GetAwaiter()
                .GetResult());

        var dailyGroupedPayments = paymentVms.GroupBy(p => p.Date.Date)
            .Select(g => new PaymentDayGroup(date: DateOnly.FromDateTime(g.Key), payments: g.ToList()))
            .ToList();

        PaymentDayGroups = new(new(dailyGroupedPayments));
    }
}
