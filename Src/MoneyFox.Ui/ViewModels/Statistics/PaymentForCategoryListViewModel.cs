namespace MoneyFox.Ui.ViewModels.Statistics;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Queries;
using Payments;
using Views.Payments;

internal sealed class PaymentForCategoryListViewModel : BaseViewModel
{
    private readonly IMapper mapper;

    private readonly IMediator mediator;

    private ObservableCollection<DateListGroupCollection<PaymentViewModel>> paymentList = new();

    public PaymentForCategoryListViewModel(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        PaymentList = new();
        IsActive = true;
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

    public RelayCommand<PaymentViewModel> GoToEditPaymentCommand
        => new(async pvm => await Shell.Current.GoToAsync($"{Routes.EditPaymentRoute}?paymentId={pvm.Id}"));

    protected override void OnActivated()
    {
        Messenger.Register<PaymentForCategoryListViewModel, PaymentsForCategoryMessage>(recipient: this, handler: (r, m) => r.InitializeAsync(m));
    }

    protected override void OnDeactivated()
    {
        Messenger.Unregister<PaymentsForCategoryMessage>(this);
    }

    private async Task InitializeAsync(PaymentsForCategoryMessage receivedMessage)
    {
        var loadedPayments = mapper.Map<List<PaymentViewModel>>(
            await mediator.Send(
                new GetPaymentsForCategoryQuery(
                    categoryId: receivedMessage.CategoryId,
                    dateRangeFrom: receivedMessage.StartDate,
                    dateRangeTo: receivedMessage.EndDate)));

        var dailyItems = DateListGroupCollection<PaymentViewModel>.CreateGroups(
            items: loadedPayments,
            getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
            getSortKey: s => s.Date);

        PaymentList = new(dailyItems);
    }
}
