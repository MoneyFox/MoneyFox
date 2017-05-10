using System;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;
using MoneyFox.Service.Pocos;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     View Model for creating and editing Categories without dialog
    /// </summary>
    public class ModifyCategoryViewModel : BaseViewModel
    {
        private readonly IBackupManager backupManager;
        private readonly ICategoryService categoryService;
        private readonly IDialogService dialogService;
        private readonly ISettingsManager settingsManager;
        private bool isEdit;

        private CategoryViewModel selectedCategory;

        public ModifyCategoryViewModel(ICategoryService categoryService, IDialogService dialogService,
                                       ISettingsManager settingsManager,
                                       IBackupManager backupManager)
        {
            this.categoryService = categoryService;
            this.dialogService = dialogService;
            this.settingsManager = settingsManager;
            this.backupManager = backupManager;
        }

        /// <summary>
        ///     Saves changes to a CategoryViewModel if in edit mode <see cref="IsEdit" />  or creates
        ///     a new CategoryViewModel.
        /// </summary>
        public MvxCommand SaveCommand => new MvxCommand(SaveCategory);

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public MvxCommand DeleteCommand => new MvxCommand(DeleteCategory);

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     The currently selected CategoryViewModel
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get { return selectedCategory; }
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
            get { return isEdit; }
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

        /// <summary>
        ///     Initialize the ViewModel
        /// </summary>
        /// <param name="categoryId">Pass the ID of the category to edit. If this is 0 the VM changes to Creation mode</param>
        public async void Init(int categoryId)
        {
            if (categoryId == 0)
            {
                IsEdit = false;
                SelectedCategory = new CategoryViewModel(new Category());
            }
            else
            {
                IsEdit = true;
                SelectedCategory = new CategoryViewModel(await categoryService.GetById(categoryId));
            }
        }

        /// <summary>
        ///     Initialize the ViewModel
        /// </summary>
        /// <param name="isEdit">Indicates if a CategoryViewModel is being edited or created</param>
        /// <param name="selectedCategoryId">If we are editing a CategoryViewModel this is its Id</param>
        public async void Init(bool isEdit, int selectedCategoryId)
        {
            IsEdit = isEdit;
            SelectedCategory = selectedCategoryId != 0
                ? new CategoryViewModel(await categoryService.GetById(selectedCategoryId))
                : new CategoryViewModel(new Category());
        }

        private async void SaveCategory()
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

            Close(this);
        }

        private async void DeleteCategory()
        {
            try
            {
                await categoryService.DeleteCategory(SelectedCategory.Category);
                settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014

                Close(this);
            }
            catch (Exception)
            {
                await dialogService.ShowMessage(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
            }
        }

        private async void Cancel()
        {
            SelectedCategory = new CategoryViewModel(await categoryService.GetById(SelectedCategory.Id));
            Close(this);
        }
    }
}