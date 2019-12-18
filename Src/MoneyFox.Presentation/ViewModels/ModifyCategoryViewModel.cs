using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Categories.Queries.GetIfCategoryWithNameExists;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Commands;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IModifyCategoryViewModel : IBaseViewModel
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
    public abstract class ModifyCategoryViewModel : BaseViewModel, IModifyCategoryViewModel
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

        protected abstract Task Initialize();

        protected abstract Task SaveCategory();

        protected INavigationService NavigationService { get; }

        protected IDialogService DialogService { get; }

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

        public AsyncCommand SaveCommand => new AsyncCommand(SaveCategoryBase);

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public AsyncCommand CancelCommand => new AsyncCommand(Cancel);

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

        private async Task SaveCategoryBase()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await DialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (await mediator.Send(new GetIfCategoryWithNameExistsQuery(SelectedCategory.Name)))
            {
                await DialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
                return;
            }

            await SaveCategory();
        }

        private async Task Cancel()
        {
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(SelectedCategory.Id)));
            NavigationService.GoBack();
        }
    }
}
