using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using MoneyFox.Business.Parameters;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     View Model for creating and editing Categories without dialog
    /// </summary>
    public class ModifyCategoryViewModel : MvxViewModel<ModifyCategoryParameter>
    {
        private readonly IBackupManager backupManager;
        private readonly ICategoryService categoryService;
        private readonly IDialogService dialogService;
        private readonly ISettingsManager settingsManager;

        protected readonly IMvxNavigationService NavigationService;

        private bool isEdit;

        private CategoryViewModel selectedCategory;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ModifyCategoryViewModel(ICategoryService categoryService, 
                                       IDialogService dialogService,
                                       ISettingsManager settingsManager,
                                       IBackupManager backupManager, 
                                       IMvxNavigationService navigationService)
        {
            this.categoryService = categoryService;
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.backupManager = backupManager;
            this.NavigationService = navigationService;
        }

        #region Commands

        /// <summary>
        ///     Saves changes to a CategoryViewModel if in edit mode <see cref="IsEdit" />  or creates
        ///     a new CategoryViewModel.
        /// </summary>
        public MvxAsyncCommand SaveCommand => new MvxAsyncCommand(SaveCategory);

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public MvxAsyncCommand CancelCommand => new MvxAsyncCommand(Cancel);

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeleteCategory);

        #endregion

        #region Properties

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

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
        ///     Indicates if the selected CategoryViewModel shall be edited or a new one created.
        /// </summary>
        public bool IsEdit
        {
            get => isEdit;
            set
            {
                isEdit = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns the Title based on whether a CategoryViewModel is being created or edited
        /// </summary>
        public string Title => IsEdit
            ? string.Format(Strings.EditCategoryTitle, SelectedCategory.Name)
            : Strings.AddCategoryTitle;

        #endregion

        private int categoryId;

        /// <inheritdoc />
        public override void Prepare(ModifyCategoryParameter parameter)
        {
            categoryId = parameter.CategoryId;
        }

        /// <inheritdoc />
        public override async Task Initialize()
        {
            if (categoryId == 0)
            {
                IsEdit = false;
                SelectedCategory = new CategoryViewModel(new Category());
            } else
            {
                IsEdit = true;
                SelectedCategory = new CategoryViewModel(await categoryService.GetById(categoryId));
            }
        }

        private async Task SaveCategory()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (!IsEdit && await categoryService.CheckIfNameAlreadyTaken(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.ErrorMessageSave, Strings.DuplicateCategoryMessage);
                return;
            }

            await categoryService.SaveCategory(SelectedCategory.Category);
            settingsManager.LastDatabaseUpdate = DateTime.Now;

#pragma warning disable 4014
            backupManager.EnqueueBackupTask();
#pragma warning restore 4014

            await NavigationService.Close(this);
        }

        private async Task DeleteCategory()
        {
            try
            {
                await categoryService.DeleteCategory(SelectedCategory.Category);
                settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014

                await NavigationService.Close(this);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await dialogService.ShowMessage(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
            }
        }

        private async Task Cancel()
        {
            SelectedCategory = new CategoryViewModel(await categoryService.GetById(SelectedCategory.Id));
            await NavigationService.Close(this);
        }
    }
}