using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoneyFox.Core.Constants;
using MoneyFox.Core.Groups;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Model;
using MoneyFox.Core.Resources;
using IDialogService = MoneyFox.Core.Interfaces.IDialogService;

namespace MoneyFox.Core.ViewModels
{
    public class RecurringPaymentListViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly INavigationService navigationService;
        private readonly IPaymentRepository paymentRepository;

        public RecurringPaymentListViewModel(IPaymentRepository paymentRepository,
            IDialogService dialogService,
            INavigationService navigationService)
        {
            this.paymentRepository = paymentRepository;
            this.dialogService = dialogService;
            this.navigationService = navigationService;

            AllPaymentViewModels = new ObservableCollection<PaymentViewModel>();
        }

        public ObservableCollection<PaymentViewModel> AllPaymentViewModels { get; }

        /// <summary>
        ///     Returns groupped related PaymentViewModels
        /// </summary>
        public ObservableCollection<AlphaGroupListGroup<PaymentViewModel>> Source { get; private set; }

        /// <summary>
        ///     Prepares the recurring PaymentViewModel list view
        /// </summary>
        public RelayCommand LoadedCommand => new RelayCommand(Loaded);

        /// <summary>
        ///     Edits the currently selected PaymentViewModel.
        /// </summary>
        public RelayCommand<PaymentViewModel> EditCommand { get; private set; }

        /// <summary>
        ///     Deletes the selected PaymentViewModel
        /// </summary>
        public RelayCommand<PaymentViewModel> DeleteCommand => new RelayCommand<PaymentViewModel>(Delete);

        private void Loaded()
        {
            EditCommand = null;

            AllPaymentViewModels.Clear();
            foreach (var paymentViewModel in paymentRepository.LoadRecurringList())
            {
                AllPaymentViewModels.Add(paymentViewModel);
            }

            Source = new ObservableCollection<AlphaGroupListGroup<PaymentViewModel>>(
                AlphaGroupListGroup<PaymentViewModel>.CreateGroups(AllPaymentViewModels,
                    CultureInfo.CurrentUICulture,
                    s => s.ChargedAccount.Name));

            //We have to set the command here to ensure that the selection changed event is triggered earlier
            EditCommand = new RelayCommand<PaymentViewModel>(Edit);
        }

        private void Edit(PaymentViewModel paymentViewModel)
        {
            paymentRepository.Selected = paymentViewModel;

            navigationService.NavigateTo(NavigationConstants.MODIFY_PAYMENT_VIEW, paymentViewModel);
        }

        private async void Delete(PaymentViewModel paymentViewModel)
        {
            if (!await
                dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeletePaymentViewModelConfirmationMessage))
            {
                return;
            }

            paymentRepository.Delete(paymentViewModel);
            LoadedCommand.Execute(null);
        }
    }
}