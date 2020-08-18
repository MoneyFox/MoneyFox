using AutoMapper;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Accounts.Queries.GetAccounts;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Common;
using MoneyFox.Extensions;
using MoneyFox.Groups;
using MoneyFox.Views.Accounts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Accounts
{
    public class AccountListViewModel : BaseViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> accounts = new ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>>();

        public AccountListViewModel(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            MessengerInstance.Register<ReloadMessage>(this, async (m) => await OnAppearingAsync());
        }

        public async Task OnAppearingAsync()
        {
            Accounts.Clear();

            var accountVms = mapper.Map<List<AccountViewModel>>(await mediator.Send(new GetAccountsQuery()));

            var includedAccountGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.IncludedAccountsHeader);
            var excludedAccountGroup = new AlphaGroupListGroupCollection<AccountViewModel>(Strings.ExcludedAccountsHeader);

            includedAccountGroup.AddRange(accountVms.Where(x => !x.IsExcluded));
            excludedAccountGroup.AddRange(accountVms.Where(x => x.IsExcluded));

            if(includedAccountGroup.Any())
            {
                Accounts.Add(includedAccountGroup);
            }

            if(excludedAccountGroup.Any())
            {
                Accounts.Add(excludedAccountGroup);
            }
        }

        public ObservableCollection<AlphaGroupListGroupCollection<AccountViewModel>> Accounts
        {
            get => accounts;
            set
            {
                accounts = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand GoToAddAccountCommand => new RelayCommand(async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddAccountRoute));

        public RelayCommand<AccountViewModel> GoToEditAccountCommand
            => new RelayCommand<AccountViewModel>(async (accountViewModel)
                => await Shell.Current.Navigation.PushModalAsync(
                    new NavigationPage(new EditAccountPage(accountViewModel.Id)) { BarBackgroundColor = Color.Transparent, BarTextColor = ResourceHelper.GetCurrentTextColor() }));

        public RelayCommand<AccountViewModel> GoToTransactionListCommand
            => new RelayCommand<AccountViewModel>(async (accountViewModel)
                => await Shell.Current.GoToAsync($"{ViewModelLocator.PaymentListRoute}?accountId={accountViewModel.Id}"));
    }
}
