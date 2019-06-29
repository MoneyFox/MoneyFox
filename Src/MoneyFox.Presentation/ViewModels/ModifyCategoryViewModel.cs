using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.Utilities;

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
        private readonly ICrudServicesAsync crudServices;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupService backupService;

        private CategoryViewModel selectedCategory;
        private string title;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected ModifyCategoryViewModel(ICrudServicesAsync crudServices,
                                       ISettingsFacade settingsFacade,
                                       IBackupService backupService,
                                       INavigationService navigationService)
        {
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.crudServices = crudServices;

            NavigationService = navigationService;
        }

        protected INavigationService NavigationService { get; }

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
        public string Title {
            get => title;
            set
            {
                if (title == value) return;
                title = value;
                RaisePropertyChanged();
            }
        }

        public int CategoryId { get; set; }

        protected abstract Task SaveCategory();

        private async Task SaveCategoryBase()
        {
            await SaveCategory();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            if (settingsFacade.IsBackupAutouploadEnabled)
            {
                backupService.EnqueueBackupTask().FireAndForgetSafeAsync();
            }
        }

        private async Task Cancel()
        {
            SelectedCategory = await crudServices.ReadSingleAsync<CategoryViewModel>(SelectedCategory.Id);
            NavigationService.GoBack();
        }
    }
}