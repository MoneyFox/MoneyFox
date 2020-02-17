using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight;
using MediatR;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Categories.Queries.GetIfCategoryWithNameExists;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Services;
using MoneyFox.Ui.Shared.Commands;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IModifyCategoryViewModel
    {
        /// <summary>
        ///     Saves changes to a CategoryViewModel
        /// </summary>
        AsyncCommand SaveCommand { get; }

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        AsyncCommand CancelCommand { get; }

        /// <summary>
        ///     Selected category.
        /// </summary>
        CategoryViewModel SelectedCategory { get; }
    }

    /// <summary>
    ///     View Model for creating and editing Categories without dialog
    /// </summary>
    public abstract class ModifyCategoryViewModel : ViewModelBase, IModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        private CategoryViewModel selectedCategory;
        private string title;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected ModifyCategoryViewModel(IMediator mediator,
                                          INavigationService navigationService,
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

        protected INavigationService NavigationService { get; }

        protected IDialogService DialogService { get; }

        public AsyncCommand InitializeCommand => new AsyncCommand(InitializeAsync);

        public AsyncCommand SaveCommand => new AsyncCommand(SaveCategoryBaseAsync);

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public AsyncCommand CancelCommand => new AsyncCommand(CancelAsync);

        /// <summary>
        ///     The currently selected CategoryViewModel
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
        ///     Returns the Title based on whether a CategoryViewModel is being created or edited
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if (title == value) return;
                title = value;
                RaisePropertyChanged();
            }
        }

        public int CategoryId { get; set; }

        private async Task SaveCategoryBaseAsync()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (await mediator.Send(new GetIfCategoryWithNameExistsQuery(SelectedCategory.Name)))
            {
                await DialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
                return;
            }

            await SaveCategoryAsync();
        }

        private async Task CancelAsync()
        {
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(SelectedCategory.Id)));
            NavigationService.GoBackModal();
        }
    }
}
