namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using MediatR;
using Payments;
using Resources.Strings;

internal sealed class PaymentForCategoryListViewModel : BasePageViewModel, IRecipient<PaymentsForCategoryMessage>
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    private ObservableCollection<DateListGroupCollection<PaymentViewModel>> paymentList = new();
    private string title = string.Empty;

    public PaymentForCategoryListViewModel(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        PaymentList = new();
    }

    public string Title
    {
        get => title;
        set => SetProperty(field: ref title, newValue: value);
    }

    public ObservableCollection<DateListGroupCollection<PaymentViewModel>> PaymentList
    {
        get => paymentList;

        private set
        {
            paymentList = value;
            OnPropertyChanged();
        }
    }

    public AsyncRelayCommand<PaymentViewModel> GoToEditPaymentCommand
        => new(async pvm => await Shell.Current.GoToAsync($"{Routes.EditPaymentRoute}?paymentId={pvm!.Id}"));

    public async void Receive(PaymentsForCategoryMessage message)
    {
        if (message.CategoryId.HasValue)
        {
            var category = await mediator.Send(new GetCategoryByIdQuery(message.CategoryId.Value));
            Title = string.Format(format: Translations.PaymentsForCategoryTitle, arg0: category.Name);
        }
        else
        {
            Title = Translations.NoCategoryTitle;
        }

        var loadedPayments = mapper.Map<List<PaymentViewModel>>(
            await mediator.Send(
                new GetPaymentsForCategorySummary.Query(categoryId: message.CategoryId, dateRangeFrom: message.StartDate, dateRangeTo: message.EndDate)));

        var dailyItems = DateListGroupCollection<PaymentViewModel>.CreateGroups(
            items: loadedPayments,
            getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
            getSortKey: s => s.Date);

        PaymentList = new(dailyItems);
    }
}
