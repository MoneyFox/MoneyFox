namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using MediatR;
using MoneyFox.Ui.Views.Payments.PaymentList;
using Resources.Strings;

internal sealed class PaymentForCategoryListViewModel : BasePageViewModel, IRecipient<PaymentsForCategoryMessage>
{
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    private ObservableCollection<DateListGroupCollection<PaymentListItemViewModel>> paymentList = new();
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
        set => SetProperty( ref title,   value);
    }

    public ObservableCollection<DateListGroupCollection<PaymentListItemViewModel>> PaymentList
    {
        get => paymentList;

        private set
        {
            paymentList = value;
            OnPropertyChanged();
        }
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

        var loadedPayments = mapper.Map<List<PaymentListItemViewModel>>(
            mediator.Send(new GetPaymentsForCategorySummary.Query(CategoryId: message.CategoryId, DateRangeFrom: message.StartDate, DateRangeTo: message.EndDate)).GetAwaiter().GetResult());

        var dailyItems = DateListGroupCollection<PaymentListItemViewModel>.CreateGroups(
            items: loadedPayments,
            getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
            getSortKey: s => s.Date);

        PaymentList = new(dailyItems);
    }
}
