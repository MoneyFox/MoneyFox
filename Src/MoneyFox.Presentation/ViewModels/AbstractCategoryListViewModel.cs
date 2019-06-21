using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.QueryObject;
using IDialogService = MoneyFox.ServiceLayer.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public abstract class AbstractCategoryListViewModel : BaseViewModel
    {
        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> source;


        /// <summary>
        ///     Base class for the category list user control
        /// </summary>
        protected AbstractCategoryListViewModel(ICrudServicesAsync crudServices,
                                                IDialogService dialogService,
                                                INavigationService navigationService)
        {
            CrudServices = crudServices;
            DialogService = dialogService;
            NavigationService = navigationService;
        }

        protected INavigationService NavigationService { get; private set; }

        protected ICrudServicesAsync CrudServices { get; }
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

        public RelayCommand AppearingCommand => new RelayCommand(ViewAppearing);

        /// <summary>
        ///     Deletes the passed CategoryViewModel after show a confirmation dialog.
        /// </summary>
        public RelayCommand<CategoryViewModel> DeleteCategoryCommand => new RelayCommand<CategoryViewModel>(DeleteCategory);

        /// <summary>
        ///     Edit the currently selected CategoryViewModel
        /// </summary>
        public RelayCommand<CategoryViewModel> EditCategoryCommand => new RelayCommand<CategoryViewModel>(EditCategory);

        /// <summary>
        ///     Selects the clicked CategoryViewModel and sends it to the message hub.
        /// </summary>
        public RelayCommand<CategoryViewModel> ItemClickCommand  => new RelayCommand<CategoryViewModel>(ItemClick);

        /// <summary>
        ///     Executes a search for the passed term and updates the displayed list.
        /// </summary>
        public RelayCommand<string> SearchCommand => new RelayCommand<string>(Search);

        /// <summary>
        ///     Create and save a new CategoryViewModel group
        /// </summary>
        public RelayCommand<CategoryViewModel> CreateNewCategoryCommand => new RelayCommand<CategoryViewModel>(CreateNewCategory);

        public async void ViewAppearing()
        {
            DialogService.ShowLoadingDialog();
            await Task.Run(Load);
            DialogService.HideLoadingDialog();
        }

        /// <summary>
        ///     Performs a search with the text in the search text property
        /// </summary>
        public async void Search(string searchText = "")
        {
            List<CategoryViewModel> categories;

            var categoryQuery = CrudServices
                .ReadManyNoTracked<CategoryViewModel>()
                .OrderBy(x => x.Name);

            if (!string.IsNullOrEmpty(searchText))
            {
                categories = new List<CategoryViewModel>(
                    await categoryQuery
                        .WhereNameContains(searchText)
                        .ToListAsync());
            } 
            else
            {
                categories = new List<CategoryViewModel>(
                    await categoryQuery
                        .ToListAsync());
            }
            CategoryList = CreateGroup(categories);
        }

        private void Load()
        {
            Search();
        }

        private void EditCategory(CategoryViewModel category)
        {
            NavigationService.NavigateTo(ViewModelLocator.EditCategory, category.Id);
        }

        private void CreateNewCategory(CategoryViewModel category)
        {
            NavigationService.NavigateTo(ViewModelLocator.EditCategory);
        }

        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CreateGroup(List<CategoryViewModel> categories) =>
            new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>(
                AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(categories,
                    CultureInfo.CurrentUICulture,
                    s => string.IsNullOrEmpty(s.Name)
                        ? "-"
                        : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture), itemClickCommand: ItemClickCommand));

        private async void DeleteCategory(CategoryViewModel categoryToDelete)
        {
            if (await DialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                await CrudServices.DeleteAndSaveAsync<Category>(categoryToDelete.Id);
                Search();
            }
        }
    }
}