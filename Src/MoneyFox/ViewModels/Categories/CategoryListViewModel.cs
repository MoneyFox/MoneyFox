using AutoMapper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Categories.Queries.GetCategoryBySearchTerm;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Application.Resources;
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
    public class CategoryListViewModel : ViewModelBase
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
                RaisePropertyChanged();
            }
        }

        public RelayCommand GoToAddCategoryCommand => new RelayCommand(
            async () => await Shell.Current.GoToModalAsync(ViewModelLocator.AddCategoryRoute));

        public RelayCommand<string> SearchCategoryCommand
            => new RelayCommand<string>(async searchTerm => await SearchCategoryAsync(searchTerm));

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                searchTerm = value;
                SearchCategoryCommand.Execute(searchTerm);
            }
        }

        public RelayCommand<CategoryViewModel> GoToEditCategoryCommand
            => new RelayCommand<CategoryViewModel>(
                async categoryViewModel
                    => await Shell.Current.Navigation.PushModalAsync(
                        new NavigationPage(new EditCategoryPage(categoryViewModel.Id))
                        {
                            BarBackgroundColor = Color.Transparent
                        }));


        public RelayCommand<CategoryViewModel> DeleteCategoryCommand
            => new RelayCommand<CategoryViewModel>(
                async categoryViewModel
                    => await DeleteAccountAsync(categoryViewModel));

        public void Subscribe()
            => MessengerInstance.Register<ReloadMessage>(this, async m => await InitializeAsync());

        public void Unsubscribe()
            => MessengerInstance.Unregister<ReloadMessage>(this);

        public async Task InitializeAsync() => await SearchCategoryAsync();

        private async Task SearchCategoryAsync(string searchTerm = "")
        {
            var categorieVms =
                mapper.Map<List<CategoryViewModel>>(await mediator.Send(new GetCategoryBySearchTermQuery(searchTerm)));

            var groups =
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