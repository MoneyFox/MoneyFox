using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.Presentation.Services;
using MoneyFox.ServiceLayer.Facades;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IModifyCategoryViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Saves changes to a CategoryViewModel if in edit mode <see cref="IsEdit" />  or creates
        ///     a new CategoryViewModel.
        /// </summary>
        RelayCommand SaveCommand { get; }
    
        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        RelayCommand CancelCommand { get; }

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
        protected abstract Task SaveCategory();

        private async void SaveCategoryBase()
        {
            await SaveCategory();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            if (settingsFacade.IsBackupAutouploadEnabled)
            {
#pragma warning disable 4014
                backupService.EnqueueBackupTask();
#pragma warning restore 4014
            }
        }

        public RelayCommand SaveCommand => new RelayCommand(SaveCategoryBase);

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public RelayCommand CancelCommand => new RelayCommand(Cancel);

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
        public string Title { get; set; }

        public int CategoryId { get; set; }

        private async void Cancel()
        {
            SelectedCategory = await crudServices.ReadSingleAsync<CategoryViewModel>(SelectedCategory.Id);
            NavigationService.GoBack();
        }
    }
}