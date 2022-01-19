using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.Messages;
using MoneyFox.Core.Commands.Categories.DeleteCategoryById;
using MoneyFox.Core.Queries.Categories.GetCategoryBySearchTerm;
using MoneyFox.Core.Resources;
using MoneyFox.Uwp.Groups;
using MoneyFox.Uwp.Services;
using MoneyFox.Uwp.Views.Categories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Categories
{
    public abstract class AbstractCategoryListViewModel : ObservableRecipient
    {
        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> source =
            new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>();

        /// <summary>
        ///     Base class for the category list user control
        /// </summary>
        protected AbstractCategoryListViewModel(IMediator mediator,
            IMapper mapper,
            IDialogService dialogService,
            NavigationService navigationService)
        {
            Mediator = mediator;
            Mapper = mapper;
            DialogService = dialogService;
            NavigationService = navigationService;
            IsActive = true;
        }

        protected NavigationService NavigationService { get; }

        protected IMediator Mediator { get; }

        protected IMapper Mapper { get; }

        protected IDialogService DialogService { get; }

        /// <summary>
        ///     Collection with categories alphanumeric grouped by
        /// </summary>
        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList
        {
            get => source;
            private set
            {
                if(source == value)
                {
                    return;
                }

                source = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCategoriesEmpty));
            }
        }

        public bool IsCategoriesEmpty => !CategoryList?.Any() ?? true;

        public RelayCommand AppearingCommand => new RelayCommand(async () => await ViewAppearingAsync());

        /// <summary>
        ///     Deletes the passed CategoryViewModel after show a confirmation dialog.
        /// </summary>
        public AsyncRelayCommand<CategoryViewModel> DeleteCategoryCommand
            => new AsyncRelayCommand<CategoryViewModel>(DeleteCategoryAsync);

        /// <summary>
        ///     Edit the currently selected CategoryViewModel
        /// </summary>
        public RelayCommand<CategoryViewModel> EditCategoryCommand
            => new RelayCommand<CategoryViewModel>(
                async vm => await new EditCategoryDialog(vm.Id) { RequestedTheme = ThemeSelectorService.Theme }
                    .ShowAsync());

        /// <summary>
        ///     Selects the clicked CategoryViewModel and sends it to the message hub.
        /// </summary>
        public RelayCommand<CategoryViewModel> ItemClickCommand => new RelayCommand<CategoryViewModel>(ItemClick);

        /// <summary>
        ///     Executes a search for the passed term and updates the displayed list.
        /// </summary>
        public AsyncRelayCommand<string> SearchCommand => new AsyncRelayCommand<string>(SearchAsync);

        protected override void OnActivated() =>
            Messenger.Register<AbstractCategoryListViewModel, ReloadMessage>(
                this,
                (r, m) => r.SearchCommand.ExecuteAsync(""));

        protected override void OnDeactivated() => Messenger.Unregister<ReloadMessage>(this);

        /// <summary>
        ///     Handle the selection of a CategoryViewModel in the list
        /// </summary>
        protected abstract void ItemClick(CategoryViewModel category);

        public async Task ViewAppearingAsync() => await SearchAsync();

        /// <summary>
        ///     Performs a search with the text in the search text property
        /// </summary>
        public async Task SearchAsync(string searchText = "")
        {
            var categoriesVms =
                Mapper.Map<List<CategoryViewModel>>(await Mediator.Send(new GetCategoryBySearchTermQuery(searchText)));
            CategoryList = CreateGroup(categoriesVms);
        }

        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CreateGroup(
            IEnumerable<CategoryViewModel> categories) =>
            new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>(
                AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(
                    categories,
                    CultureInfo.CurrentUICulture,
                    s => string.IsNullOrEmpty(s.Name)
                        ? "-"
                        : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture),
                    itemClickCommand: ItemClickCommand));

        private async Task DeleteCategoryAsync(CategoryViewModel categoryToDelete)
        {
            if(await DialogService.ShowConfirmMessageAsync(
                   Strings.DeleteTitle,
                   Strings.DeleteCategoryConfirmationMessage))
            {
                await Mediator.Send(new DeleteCategoryByIdCommand(categoryToDelete.Id));
                await SearchAsync();
            }
        }
    }
}