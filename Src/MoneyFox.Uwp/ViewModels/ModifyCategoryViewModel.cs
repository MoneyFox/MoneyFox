using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Categories.Queries.GetIfCategoryWithNameExists;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.Services;
using System.Threading.Tasks;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Uwp.ViewModels
{
    /// <summary>
    /// View Model for creating and editing Categories without dialog
    /// </summary>
    public abstract class ModifyCategoryViewModel : ViewModelBase, IModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        private CategoryViewModel selectedCategory;
        private string title;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ModifyCategoryViewModel(IMediator mediator,
                                          NavigationService navigationService,
                                          IMapper mapper,
                                          IDialogService dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            NavigationService = navigationService;
            DialogService = dialogService;
        }

        protected abstract Task InitializeAsync();

        protected abstract Task SaveCategoryAsync();

        protected NavigationService NavigationService { get; }

        protected IDialogService DialogService { get; }

        public AsyncCommand InitializeCommand => new AsyncCommand(InitializeAsync);

        public AsyncCommand SaveCommand => new AsyncCommand(SaveCategoryBaseAsync);

        /// <summary>
        /// Cancel the current operation
        /// </summary>
        public AsyncCommand CancelCommand => new AsyncCommand(CancelAsync);

        /// <summary>
        /// The currently selected CategoryViewModel
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Returns the Title based on whether a CategoryViewModel is being created or edited
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if(title == value)
                    return;
                title = value;
                RaisePropertyChanged();
            }
        }

        public int CategoryId { get; set; }

        private async Task SaveCategoryBaseAsync()
        {
            if(string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if(await mediator.Send(new GetIfCategoryWithNameExistsQuery(SelectedCategory.Name)))
            {
                await DialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
                return;
            }

            await DialogService.ShowLoadingDialogAsync(Strings.SavingCategoryMessage);
            await SaveCategoryAsync();
            MessengerInstance.Send(new ReloadMessage());
            await DialogService.HideLoadingDialogAsync();
        }

        private async Task CancelAsync()
        {
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(SelectedCategory.Id)));
        }
    }
}
