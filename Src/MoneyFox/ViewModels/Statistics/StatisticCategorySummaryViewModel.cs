namespace MoneyFox.ViewModels.Statistics
{
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core.Common.Interfaces;
    using Extensions;
    using MediatR;
    using NLog;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Queries.Statistics.GetCategorySummary;
    using Serilog;
    using Xamarin.Forms;

    public class StatisticCategorySummaryViewModel : StatisticViewModel
    {
        private readonly IDialogService dialogService;

        private ObservableCollection<CategoryOverviewViewModel> categorySummary =
            new ObservableCollection<CategoryOverviewViewModel>();

        public StatisticCategorySummaryViewModel(
            IMediator mediator,
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
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasData));
            }
        }

        public bool HasData => CategorySummary.Any();

        public RelayCommand<CategoryOverviewViewModel> ShowCategoryPaymentsCommand
            => new RelayCommand<CategoryOverviewViewModel>(async vm => await ShowCategoryPaymentsAsync(vm));

        protected override async Task LoadAsync()
        {
            try
            {
                CategorySummaryModel categorySummaryModel =
                    await Mediator.Send(new GetCategorySummaryQuery { EndDate = EndDate, StartDate = StartDate });

                CategorySummary = new ObservableCollection<CategoryOverviewViewModel>(
                    categorySummaryModel
                        .CategoryOverviewItems
                        .Select(
                            x => new CategoryOverviewViewModel
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
                Log.Warning(ex, "Error during loading");
                await dialogService.ShowMessageAsync("Error", ex.ToString());
            }
        }

        private async Task ShowCategoryPaymentsAsync(CategoryOverviewViewModel categoryOverviewModel)
        {
            await Shell.Current.GoToModalAsync(ViewModelLocator.PaymentForCategoryListRoute);
            Messenger.Send(
                new PaymentsForCategoryMessage(
                    categoryOverviewModel.CategoryId,
                    StartDate,
                    EndDate));
        }
    }
}
