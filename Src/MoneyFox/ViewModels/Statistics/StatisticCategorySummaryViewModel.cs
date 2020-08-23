using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Statistics.Queries.GetCategorySummary;
using MoneyFox.Extensions;
using NLog;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Statistics
{
    /// <inheritdoc cref="IStatisticCategorySummaryViewModel"/>
    public class StatisticCategorySummaryViewModel : MobileStatisticViewModel
    {
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IDialogService dialogService;

        private ObservableCollection<CategoryOverviewViewModel> categorySummary = new ObservableCollection<CategoryOverviewViewModel>();

        public StatisticCategorySummaryViewModel(IMediator mediator,
                                                 IDialogService dialogService) : base(mediator)
        {
            this.dialogService = dialogService;

            CategorySummary = new ObservableCollection<CategoryOverviewViewModel>();
        }

        public ObservableCollection<CategoryOverviewViewModel> CategorySummary
        {
            get => categorySummary;
            private set
            {
                categorySummary = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasData));
            }
        }

        /// <inheritdoc/>
        public bool HasData => CategorySummary.Any();

        public RelayCommand<CategoryOverviewViewModel> ShowCategoryPaymentsCommand
            => new RelayCommand<CategoryOverviewViewModel>(async (vm) => await ShowCategoryPaymentsAsync(vm));

        /// <summary>
        /// Overrides the load method to load the category summary data.
        /// </summary>
        protected override async Task LoadAsync()
        {
            try
            {
                CategorySummaryModel categorySummaryModel =
                await Mediator.Send(new GetCategorySummaryQuery { EndDate = EndDate, StartDate = StartDate });

                CategorySummary = new ObservableCollection<CategoryOverviewViewModel>(categorySummaryModel
                                                                                         .CategoryOverviewItems
                                                                                         .Select(x => new CategoryOverviewViewModel
                                                                                         {
                                                                                             CategoryId = x.CategoryId,
                                                                                             Value = x.Value,
                                                                                             Average = x.Average,
                                                                                             Label = x.Label,
                                                                                             Percentage = x.Percentage
                                                                                         }));
            }
            catch(Exception ex)
            {
                logger.Warn(ex, "Error during loading. {1}", ex);
                await dialogService.ShowMessageAsync("Error", ex.ToString());
            }
        }

        private async Task ShowCategoryPaymentsAsync(CategoryOverviewViewModel categoryOverviewModel)
        {
            await Shell.Current.GoToModalAsync(ViewModelLocator.PaymentForCategoryListRoute);
            MessengerInstance.Send(new PaymentsForCategoryMessage(categoryOverviewModel.CategoryId, StartDate, EndDate));
        }
    }
}
