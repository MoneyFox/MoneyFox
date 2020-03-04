using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Services;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using NLog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.Models;

namespace MoneyFox.Presentation.ViewModels
{
    public abstract class AbstractCategoryListViewModel : ViewModelBase
    {
        private const int MENU_RESULT_EDIT_INDEX = 0;
        private const int MENU_RESULT_DELETE_INDEX = 1;

        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> source;

        /// <summary>
        /// Base class for the category list user control
        /// </summary>
        protected AbstractCategoryListViewModel(IMediator mediator,
                                                IMapper mapper,
                                                IDialogService dialogService,
                                                INavigationService navigationService)
        {
            Mediator = mediator;
            Mapper = mapper;
            DialogService = dialogService;
            NavigationService = navigationService;
        }

        protected INavigationService NavigationService { get; }

        protected IMediator Mediator { get; }

        protected IMapper Mapper { get; }

        protected IDialogService DialogService { get; }

        /// <summary>
        /// Handle the selection of a CategoryViewModel in the list
        /// </summary>
        protected abstract void ItemClick(CategoryViewModel category);

        /// <summary>
        /// Collection with categories alphanumeric grouped by
        /// </summary>
        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList
        {
            get => source;
            private set
            {
                if(source == value)
                    return;
                source = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsCategoriesEmpty));
            }
        }

        public bool IsCategoriesEmpty => !CategoryList?.Any() ?? true;

        public List<string> MenuActions => new List<string> { Strings.EditLabel, Strings.DeleteLabel };

        public Command<MaterialMenuResult> MenuSelectedCommand => new Command<MaterialMenuResult>(MenuSelected);

        public AsyncCommand AppearingCommand => new AsyncCommand(ViewAppearingAsync);

        /// <summary>
        /// Deletes the passed CategoryViewModel after show a confirmation dialog.
        /// </summary>
        public AsyncCommand<CategoryViewModel> DeleteCategoryCommand => new AsyncCommand<CategoryViewModel>(DeleteCategoryAsync);

        /// <summary>
        /// Edit the currently selected CategoryViewModel
        /// </summary>
        public RelayCommand<CategoryViewModel> EditCategoryCommand => new RelayCommand<CategoryViewModel>(EditCategory);

        /// <summary>
        /// Selects the clicked CategoryViewModel and sends it to the message hub.
        /// </summary>
        public RelayCommand<CategoryViewModel> ItemClickCommand => new RelayCommand<CategoryViewModel>(ItemClick);

        /// <summary>
        /// Executes a search for the passed term and updates the displayed list.
        /// </summary>
        public AsyncCommand<string> SearchCommand => new AsyncCommand<string>(SearchAsync);

        /// <summary>
        /// Create and save a new CategoryViewModel group
        /// </summary>
        public RelayCommand<CategoryViewModel> CreateNewCategoryCommand => new RelayCommand<CategoryViewModel>(CreateNewCategory);

        public async Task ViewAppearingAsync()
        {
            await SearchAsync();
        }

        /// <summary>
        /// Performs a search with the text in the search text property
        /// </summary>
        public async Task SearchAsync(string searchText = "")
        {
            var categoriesVms =
                Mapper.Map<List<CategoryViewModel>>(await Mediator.Send(new GetCategoryBySearchTermQuery(searchText)));
            CategoryList = CreateGroup(categoriesVms);
        }


        [SuppressMessage("Major Bug", "S3168:\"async\" methods should not return \"void\"", Justification = "Acts as event handler.>")]
        private async void MenuSelected(MaterialMenuResult menuResult)
        {
            var categoryViewModel = menuResult.Parameter as CategoryViewModel;

            switch(menuResult.Index)
            {
                case MENU_RESULT_EDIT_INDEX:
                    EditCategory(categoryViewModel);
                    break;

                case MENU_RESULT_DELETE_INDEX:
                    await DeleteCategoryAsync(categoryViewModel);
                    break;

                default:
                    logManager.Warn("Invalid Index for Menu Selected in Account List. Index: {0}", menuResult.Index);
                    break;
            }
        }

        private void EditCategory(CategoryViewModel category)
        {
            NavigationService.NavigateToModal(ViewModelLocator.EditCategory, category.Id);
        }

        private void CreateNewCategory(CategoryViewModel category)
        {
            NavigationService.NavigateToModal(ViewModelLocator.AddCategory);
        }

        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CreateGroup(
            IEnumerable<CategoryViewModel> categories)
        {
            return new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>(
                                                                                              AlphaGroupListGroupCollection<CategoryViewModel>
                                                                                                 .CreateGroups(categories,
                                                                                                               CultureInfo.CurrentUICulture,
                                                                                                               s => string
                                                                                                                      .IsNullOrEmpty(s.Name)
                                                                                                                    ? "-"
                                                                                                                    : s.Name[0]
                                                                                                                          .ToString(CultureInfo
                                                                                                                                       .InvariantCulture)
                                                                                                                          .ToUpper(CultureInfo
                                                                                                                                      .InvariantCulture),
                                                                                                               itemClickCommand:
                                                                                                               ItemClickCommand));
        }

        private async Task DeleteCategoryAsync(CategoryViewModel categoryToDelete)
        {
            if(await DialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                await Mediator.Send(new DeleteCategoryByIdCommand(categoryToDelete.Id));
                await SearchAsync();
            }
        }
    }
}
