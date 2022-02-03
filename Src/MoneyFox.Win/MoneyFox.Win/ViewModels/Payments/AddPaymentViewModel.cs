using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Exceptions;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Commands.Payments.CreatePayment;
using MoneyFox.Core.Queries.Accounts.GetAccountById;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Services;
using MoneyFox.Win.Utilities;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyFox.Win.ViewModels.Payments
{
    public class AddPaymentViewModel : ModifyPaymentViewModel
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IDialogService dialogService;

        public AddPaymentViewModel(
            IMediator mediator,
            IMapper mapper,
            IDialogService dialogService,
            INavigationService navigationService) : base(mediator, mapper, dialogService, navigationService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
        }

        public PaymentType PaymentType { get; set; }

        public RelayCommand InitializeCommand => new RelayCommand(async () => await InitializeAsync());

        protected override async Task InitializeAsync()
        {
            Title = PaymentTypeHelper.GetViewTitleForType(PaymentType, false);
            AmountString = HelperFunctions.FormatLargeNumbers(SelectedPayment.Amount);
            SelectedPayment.Type = PaymentType;

            await base.InitializeAsync();

            SelectedPayment.ChargedAccount = ChargedAccounts.FirstOrDefault();

            if(SelectedPayment.IsTransfer)
            {
                SelectedItemChangedCommand.Execute(null);
                SelectedPayment.TargetAccount = TargetAccounts.FirstOrDefault();
            }
        }

        protected override async Task SavePaymentAsync()
        {
            try
            {
                IsBusy = true;
                var payment = new Payment(
                    SelectedPayment.Date,
                    SelectedPayment.Amount,
                    SelectedPayment.Type,
                    await mediator.Send(new GetAccountByIdQuery(
                        SelectedPayment.ChargedAccount.Id)),
                        SelectedPayment.TargetAccount != null
                            ? await mediator.Send(new GetAccountByIdQuery(SelectedPayment.TargetAccount.Id))
                            : null,
                    mapper.Map<Category>(SelectedPayment.Category),
                    SelectedPayment.Note);

                if(SelectedPayment.IsRecurring && SelectedPayment.RecurringPayment != null)
                {
                    payment.AddRecurringPayment(
                        SelectedPayment.RecurringPayment.Recurrence,
                        SelectedPayment.RecurringPayment.IsEndless
                            ? null
                            : SelectedPayment.RecurringPayment.EndDate);
                }

                await mediator.Send(new CreatePaymentCommand(payment));
            }
            catch(InvalidEndDateException)
            {
                await dialogService.ShowMessageAsync(Strings.InvalidEnddateTitle, Strings.InvalidEnddateMessage);
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}