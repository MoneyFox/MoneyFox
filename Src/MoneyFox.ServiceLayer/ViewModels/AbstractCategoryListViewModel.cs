using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.QueryObject;
using NLog;
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public abstract class AbstractCategoryListViewModel : RouteableViewModelBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string searchTerm;

        private ObservableAsPropertyHelper<bool> hasNoCategories;

        private string searchTerm;

        /// <summary>
        ///     Base class for the category list user control
        /// </summary>
        protected AbstractCategoryListViewModel(ICrudServicesAsync crudServices = null,
                                                IDialogService dialogService = null)
        {
            CrudServices = crudServices ?? Locator.Current.GetService<ICrudServicesAsync>();
            DialogService = dialogService ?? Locator.Current.GetService<IDialogService>();

            this.WhenActivated(disposables =>
            {
                ItemClickCommand = ReactiveCommand.CreateFromTask<CategoryViewModel, Unit>(ItemClick)
                                                  .DisposeWith(disposables);
                CreateNewCategoryCommand = ReactiveCommand.Create<CategoryViewModel, Unit>(CreateNewCategory)
                                                          .DisposeWith(disposables);
                EditCategoryCommand = ReactiveCommand.Create<CategoryViewModel, Unit>(EditCategory)
                                                     .DisposeWith(disposables);
                DeleteCategoryCommand = ReactiveCommand.CreateFromTask<CategoryViewModel, Unit>(DeleteCategory)
                                                       .DisposeWith(disposables);

                categories = this.WhenAnyValue(x => x.SearchTerm)
                    .Throttle(TimeSpan.FromMilliseconds(400))
                    .Select(term => term?.Trim())
                    .DistinctUntilChanged()
                    .SelectMany(SearchCategories)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .ToProperty(this, vm => vm.CategoryList);

                categories.ThrownExceptions.Subscribe(ex =>
                {
                    Logger.Error(ex);
                    DialogService.ShowMessage(Strings.GeneralErrorTitle, Strings.GeneralErrorMessage);
                });

                hasNoCategories = this.WhenAnyValue(x => x.CategoryList)
                                      .Select(x => x != null && !x.Any())
                                      .ToProperty(this, x => x.HasNoCategories);
            });
        }

        protected ICrudServicesAsync CrudServices { get; }
        protected IDialogService DialogService { get; }

        /// <summary>
        ///     Handle the selection of a CategoryViewModel in the list
        /// </summary>
        protected abstract Task<Unit> ItemClick(CategoryViewModel category);

        private ObservableAsPropertyHelper<List<AlphaGroupListGroupCollection<CategoryViewModel>>> categories;
        public List<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList => categories.Value;

        private ObservableAsPropertyHelper<bool> hasNoCategories;
        public bool HasNoCategories => hasNoCategories.Value;

        public string SearchTerm
        {
            get => searchTerm ?? "";
            set => this.RaiseAndSetIfChanged(ref searchTerm, value);
        }

        /// <summary>
        ///     Deletes the passed CategoryViewModel after show a confirmation dialog.
        /// </summary>
        public ReactiveCommand<CategoryViewModel, Unit> DeleteCategoryCommand { get; set; }

        /// <summary>
        ///     Edit the currently selected CategoryViewModel
        /// </summary>
        public ReactiveCommand<CategoryViewModel, Unit> EditCategoryCommand { get; set; }

        /// <summary>
        ///     Selects the clicked CategoryViewModel and sends it to the message hub.
        /// </summary>
        public ReactiveCommand<CategoryViewModel, Unit> ItemClickCommand { get; set; }

        /// <summary>
        ///     Executes a search for the passed term and updates the displayed list.
        /// </summary>
        public ReactiveCommand<string, Unit> SearchCommand { get; set; }

        /// <summary>
        ///     Create and save a new CategoryViewModel group
        /// </summary>
        public ReactiveCommand<CategoryViewModel, Unit> CreateNewCategoryCommand { get; set; }

        public async Task<List<AlphaGroupListGroupCollection<CategoryViewModel>>> SearchCategories(string searchText = "")
        {
            List<CategoryViewModel> categoryViewModels;

            IOrderedQueryable<CategoryViewModel> categoryQuery = CrudServices
                                                                 .ReadManyNoTracked<CategoryViewModel>()
                                                                 .OrderBy(x => x.Name);

            if (!string.IsNullOrEmpty(searchText))
            {
                categoryViewModels = new List<CategoryViewModel>(
                    await categoryQuery
                          .WhereNameContains(searchText)
                          .ToListAsync());
            }
            else
            {
                categoryViewModels = new List<CategoryViewModel>(
                    await categoryQuery
                        .ToListAsync());
            }

            return CreateGroup(categoryViewModels);
        }

        private Unit EditCategory(CategoryViewModel category)
        {
            HostScreen.Router.Navigate.Execute(new EditCategoryViewModel(category.Id, HostScreen));
            return Unit.Default;
        }

        private Unit CreateNewCategory(CategoryViewModel category)
        {
            HostScreen.Router.Navigate.Execute(new AddCategoryViewModel(HostScreen));
            return Unit.Default;
        }

        private List<AlphaGroupListGroupCollection<CategoryViewModel>> CreateGroup(
            List<CategoryViewModel> categoryViewModels)
        {
            return AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(categoryViewModels,
                                                                                 CultureInfo.CurrentUICulture,
                                                                                 s => string.IsNullOrEmpty(s.Name)
                                                                                     ? "-"
                                                                                     : s.Name[0].ToString(
                                                                                            CultureInfo
                                                                                                .InvariantCulture)
                                                                                        .ToUpper(
                                                                                            CultureInfo
                                                                                                .InvariantCulture),
                                                                                 itemClickCommand: ItemClickCommand);
        }

        private async Task<Unit> DeleteCategory(CategoryViewModel categoryToDelete)
        {
            if (await DialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                await CrudServices.DeleteAndSaveAsync<Category>(categoryToDelete.Id);
            }

            return Unit.Default;
        }
    }
}