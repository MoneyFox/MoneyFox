using AutoMapper;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm;
using MoneyFox.Application.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Groups;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public abstract class AbstractCategoryListViewModel : BaseViewModel
    {
        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> source;

        /// <summary>
        ///     Base class for the category list user control
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

            MessengerInstance.Register<BackupRestoredMessage>(this, async message => await Search());
        }

        protected INavigationService NavigationService { get; }

        protected IMediator Mediator { get; }
        protected IMapper Mapper { get; }
        protected IDialogService DialogService { get; }

        /// <summary>
        ///     Handle the selection of a CategoryViewModel in the list
        /// </summary>
        protected abstract void ItemClick(CategoryViewModel category);

        /// <summary>
        ///     Collection with categories alphanumeric grouped by
        /// </summary>
        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList
        {
            get => source;
            private set
            {
                if (source == value) return;
                source = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsCategoriesEmpty));
            }
        }

        public bool IsCategoriesEmpty => !CategoryList?.Any() ?? true;

        public AsyncCommand AppearingCommand => new AsyncCommand(ViewAppearing);

        /// <summary>
        ///     Deletes the passed CategoryViewModel after show a confirmation dialog.
        /// </summary>
        public AsyncCommand<CategoryViewModel> DeleteCategoryCommand => new AsyncCommand<CategoryViewModel>(DeleteCategory);

        /// <summary>
        ///     Edit the currently selected CategoryViewModel
        /// </summary>
        public RelayCommand<CategoryViewModel> EditCategoryCommand => new RelayCommand<CategoryViewModel>(EditCategory);

        /// <summary>
        ///     Selects the clicked CategoryViewModel and sends it to the message hub.
        /// </summary>
        public RelayCommand<CategoryViewModel> ItemClickCommand => new RelayCommand<CategoryViewModel>(ItemClick);

        /// <summary>
        ///     Executes a search for the passed term and updates the displayed list.
        /// </summary>
        public AsyncCommand<string> SearchCommand => new AsyncCommand<string>(Search);

        /// <summary>
        ///     Create and save a new CategoryViewModel group
        /// </summary>
        public RelayCommand<CategoryViewModel> CreateNewCategoryCommand => new RelayCommand<CategoryViewModel>(CreateNewCategory);

        public async Task ViewAppearing()
        {
            await Search();
        }

        /// <summary>
        ///     Performs a search with the text in the search text property
        /// </summary>
        public async Task Search(string searchText = "")
        {
            var categoriesVms =
                Mapper.Map<List<CategoryViewModel>>(await Mediator.Send(new GetCategoryBySearchTermQuery { SearchTerm = searchText }));
            CategoryList = CreateGroup(categoriesVms);
        }

        private void EditCategory(CategoryViewModel category)
        {
            NavigationService.NavigateTo(ViewModelLocator.EditCategory, category.Id);
        }

        private void CreateNewCategory(CategoryViewModel category)
        {
            NavigationService.NavigateTo(ViewModelLocator.AddCategory);
        }

        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CreateGroup(List<CategoryViewModel> categories)
        {
            return new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>(
                AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(categories,
                                                                              CultureInfo.CurrentUICulture,
                                                                              s => string.IsNullOrEmpty(s.Name)
                                                                                  ? "-"
                                                                                  : s.Name[0].ToString(CultureInfo.InvariantCulture)
                                                                                     .ToUpper(CultureInfo.InvariantCulture),
                                                                              itemClickCommand: ItemClickCommand));
        }

        private async Task DeleteCategory(CategoryViewModel categoryToDelete)
        {
            if (await DialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                await Mediator.Send(new DeleteCategoryByIdCommand(categoryToDelete.Id));
                await Search();
            }
        }
    }
}
