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
using MoneyFox.Extensions;
using MoneyFox.Groups;
using MoneyFox.Views.Categories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MoneyFox.ViewModels.Categories
{
    public class CategoryListViewModel : ObservableRecipient
    {
        private readonly IDialogService dialogService;
        private readonly IMapper mapper;

        private readonly IMediator mediator;

        private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> categories =
            new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>();

        private string searchTerm = string.Empty;

        public CategoryListViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.dialogService = dialogService;
        }

        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> Categories
        {
            get => categories;
            private set
            {
                categories = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand GoToAddCategoryCommand => new RelayCommand(
            async () =>
                await Shell.Current.GoToModalAsync(ViewModelLocator.AddCategoryRoute));

        public AsyncRelayCommand<string> SearchCategoryCommand =>
            new AsyncRelayCommand<string>(async searchTerm => await SearchCategoryAsync(searchTerm));

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                searchTerm = value;
                SearchCategoryCommand.Execute(searchTerm);
            }
        }

        public AsyncRelayCommand<CategoryViewModel> GoToEditCategoryCommand
            => new AsyncRelayCommand<CategoryViewModel>(
                async categoryViewModel
                    => await Shell.Current.Navigation.PushModalAsync(
                        new NavigationPage(new EditCategoryPage(categoryViewModel.Id))
                        {
                            BarBackgroundColor = Color.Transparent
                        }));

        public AsyncRelayCommand<CategoryViewModel> DeleteCategoryCommand
            => new AsyncRelayCommand<CategoryViewModel>(
                async categoryViewModel
                    => await DeleteAccountAsync(categoryViewModel));

        protected override void OnActivated() => Messenger.Register<CategoryListViewModel, ReloadMessage>(
            this,
            (r, m) => r.SearchCategoryCommand.Execute(""));

        protected override void OnDeactivated() => Messenger.Unregister<ReloadMessage>(this);

        public async Task InitializeAsync()
        {
            await SearchCategoryAsync();
            IsActive = true;
        }

        private async Task SearchCategoryAsync(string searchTerm = "")
        {
            var categorieVms =
                mapper.Map<List<CategoryViewModel>>(await mediator.Send(new GetCategoryBySearchTermQuery(searchTerm)));

            List<AlphaGroupListGroupCollection<CategoryViewModel>>? groups =
                AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(
                    categorieVms,
                    CultureInfo.CurrentUICulture,
                    s => string.IsNullOrEmpty(s.Name)
                        ? "-"
                        : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture));

            Categories = new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>(groups);
        }

        private async Task DeleteAccountAsync(CategoryViewModel categoryViewModel)
        {
            if(await dialogService.ShowConfirmMessageAsync(
                   Strings.DeleteTitle,
                   Strings.DeleteCategoryConfirmationMessage,
                   Strings.YesLabel,
                   Strings.NoLabel))
            {
                await mediator.Send(new DeleteCategoryByIdCommand(categoryViewModel.Id));
                await SearchCategoryAsync();
            }
        }
    }
}