namespace MoneyFox.ViewModels.Categories
{

    using System.Threading.Tasks;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Messages;
    using Core.Common.Interfaces;
    using Core.Queries;
    using Core.Resources;
    using MediatR;
    using Xamarin.Forms;

    public abstract class ModifyCategoryViewModel : ObservableRecipient
    {
        private readonly IDialogService dialogService;
        private readonly IMediator mediator;

        private CategoryViewModel selectedCategory = new CategoryViewModel();

        protected ModifyCategoryViewModel(IMediator mediator, IDialogService dialogService)
        {
            this.mediator = mediator;
            this.dialogService = dialogService;
        }

        public AsyncRelayCommand SaveCommand => new AsyncRelayCommand(async () => await SaveCategoryBaseAsync());

        /// <summary>
        ///     The currently selected CategoryViewModel
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;

            set
            {
                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        protected abstract Task SaveCategoryAsync();

        protected virtual async Task SaveCategoryBaseAsync()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await dialogService.ShowMessageAsync(title: Strings.MandatoryFieldEmptyTitle, message: Strings.NameRequiredMessage);

                return;
            }

            if (await mediator.Send(new GetIfCategoryWithNameExistsQuery(SelectedCategory.Name)))
            {
                await dialogService.ShowMessageAsync(title: Strings.DuplicatedNameTitle, message: Strings.DuplicateCategoryMessage);

                return;
            }

            await dialogService.ShowLoadingDialogAsync(Strings.SavingCategoryMessage);
            await SaveCategoryAsync();
            Messenger.Send(new ReloadMessage());
            await dialogService.HideLoadingDialogAsync();
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }

}
