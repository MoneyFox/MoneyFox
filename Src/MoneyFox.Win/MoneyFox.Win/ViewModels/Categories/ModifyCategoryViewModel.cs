using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.Messages;
using MoneyFox.Core.Queries.Categories.GetCategoryById;
using MoneyFox.Core.Queries.Categories.GetIfCategoryWithNameExists;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Services;
using System.Threading.Tasks;

namespace MoneyFox.Win.ViewModels.Categories
{
    /// <summary>
    ///     View Model for creating and editing Categories without dialog
    /// </summary>
    public abstract class ModifyCategoryViewModel : ObservableRecipient, IModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        private CategoryViewModel selectedCategory = new CategoryViewModel();
        private string title = "";

        /// <summary>
        ///     Constructor
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

        public AsyncRelayCommand InitializeCommand => new AsyncRelayCommand(InitializeAsync);

        public AsyncRelayCommand SaveCommand => new AsyncRelayCommand(SaveCategoryBaseAsync);

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public AsyncRelayCommand CancelCommand => new AsyncRelayCommand(CancelAsync);

        /// <summary>
        ///     The currently selected CategoryViewModel
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Returns the Title based on whether a CategoryViewModel is being created or edited
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if(title == value)
                {
                    return;
                }

                title = value;
                OnPropertyChanged();
            }
        }

        public int CategoryId { get; set; }

        private async Task SaveCategoryBaseAsync()
        {
            if(await mediator.Send(new GetIfCategoryWithNameExistsQuery(SelectedCategory.Name)))
            {
                await DialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
                return;
            }

            await DialogService.ShowLoadingDialogAsync(Strings.SavingCategoryMessage);
            await SaveCategoryAsync();
            Messenger.Send(new ReloadMessage());
            await DialogService.HideLoadingDialogAsync();
        }

        private async Task CancelAsync() => SelectedCategory =
            mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(SelectedCategory.Id)));
    }
}