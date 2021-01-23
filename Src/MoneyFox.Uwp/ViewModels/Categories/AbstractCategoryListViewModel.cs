using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using MoneyFox.Ui.Shared.ViewModels.Categories;
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
    public abstract class AbstractCategoryListViewModel : ViewModelBase
    {
        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> source = new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>();

        /// <summary>
        /// Base class for the category list user control
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
        }

        public void Subscribe()
            => MessengerInstance.Register<ReloadMessage>(this, async (m) => await SearchAsync());

        public void Unsubscribe()
            => MessengerInstance.Unregister<ReloadMessage>(this);

        protected NavigationService NavigationService { get; }

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
                {
                    return;
                }

                source = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsCategoriesEmpty));
            }
        }

        public bool IsCategoriesEmpty => !CategoryList?.Any() ?? true;

        public RelayCommand AppearingCommand => new RelayCommand(async () => await ViewAppearingAsync());

        /// <summary>
        /// Deletes the passed CategoryViewModel after show a confirmation dialog.
        /// </summary>
        public AsyncCommand<CategoryViewModel> DeleteCategoryCommand
            => new AsyncCommand<CategoryViewModel>(DeleteCategoryAsync);

        /// <summary>
        /// Edit the currently selected CategoryViewModel
        /// </summary>
        public RelayCommand<CategoryViewModel> EditCategoryCommand
            => new RelayCommand<CategoryViewModel>(async (vm) => await new EditCategoryDialog(vm.Id).ShowAsync());

        /// <summary>
        /// Selects the clicked CategoryViewModel and sends it to the message hub.
        /// </summary>
        public RelayCommand<CategoryViewModel> ItemClickCommand => new RelayCommand<CategoryViewModel>(ItemClick);

        /// <summary>
        /// Executes a search for the passed term and updates the displayed list.
        /// </summary>
        public AsyncCommand<string> SearchCommand => new AsyncCommand<string>(SearchAsync);

        public async Task ViewAppearingAsync() => await SearchAsync();

        /// <summary>
        /// Performs a search with the text in the search text property
        /// </summary>
        public async Task SearchAsync(string searchText = "")
        {
            List<CategoryViewModel> categoriesVms = Mapper.Map<List<CategoryViewModel>>(await Mediator.Send(new GetCategoryBySearchTermQuery(searchText)));
            CategoryList = CreateGroup(categoriesVms);
        }

        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CreateGroup(IEnumerable<CategoryViewModel> categories)
        {
            return new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>(
                AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(categories,
                                                                              CultureInfo.CurrentUICulture,
                                                                              s => string.IsNullOrEmpty(s.Name)
                                                                                ? "-"
                                                                                : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture),
                                                                              itemClickCommand: ItemClickCommand));
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
