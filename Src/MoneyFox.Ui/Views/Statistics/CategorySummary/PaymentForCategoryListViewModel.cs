namespace MoneyFox.Ui.Views.Statistics.CategorySummary;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.Queries;
using MediatR;
using ViewModels;
using ViewModels.Payments;

internal sealed class PaymentForCategoryListViewModel : BaseViewModel, IRecipient<PaymentsForCategoryMessage>
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
    
    public async void Receive(PaymentsForCategoryMessage message)
    {
        var loadedPayments = mapper.Map<List<PaymentViewModel>>(
            await mediator.Send(
                new GetPaymentsForCategoryQuery(
                    categoryId: message.CategoryId,
                    dateRangeFrom: message.StartDate,
                    dateRangeTo: message.EndDate)));

        var dailyItems = DateListGroupCollection<PaymentViewModel>.CreateGroups(
            items: loadedPayments,
            getKey: s => s.Date.ToString(format: "D", provider: CultureInfo.CurrentCulture),
            getSortKey: s => s.Date);

        PaymentList = new(dailyItems);
    }
}
