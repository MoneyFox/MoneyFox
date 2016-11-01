using System;
using System.Linq;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Interfaces.Repositories;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     View Model for creating and editing Categories without dialog
    /// </summary>
    public class ModifyCategoryViewModel : BaseViewModel
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IDialogService dialogService;
        private readonly ISettingsManager settingsManager;
        private readonly IBackupManager backupManager;

        private CategoryViewModel selectedCategory;
        private bool isEdit;

        public ModifyCategoryViewModel(ICategoryRepository categoryRepository, IDialogService dialogService,
            ISettingsManager settingsManager, 
            IBackupManager backupManager)
        {
            this.categoryRepository = categoryRepository;
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
        /// <param name="isEdit">Indicates if a CategoryViewModel is being edited or created</param>
        public void Init(bool isEdit)
        {
            IsEdit = isEdit;

            if (!IsEdit)
            {
                SelectedCategory = new CategoryViewModel();
            }
        }

        /// <summary>
        ///     Initialize the ViewModel
        /// </summary>
        /// <param name="isEdit">Indicates if a CategoryViewModel is being edited or created</param>
        /// <param name="selectedCategoryId">If we are editing a CategoryViewModel this is its Id</param>
        public void Init(bool isEdit, int selectedCategoryId)
        {
            IsEdit = isEdit;
            SelectedCategory = selectedCategoryId != 0
                ? categoryRepository.GetList(x => x.Id == selectedCategoryId).First()
                : new CategoryViewModel();
        }

        private async void SaveCategory()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (!IsEdit &&
                categoryRepository.GetList(
                        a => string.Equals(a.Name, SelectedCategory.Name, StringComparison.CurrentCultureIgnoreCase))
                    .Any())
            {
                await dialogService.ShowMessage(Strings.ErrorMessageSave, Strings.DuplicateCategoryMessage);
                return;
            }

            if (categoryRepository.Save(SelectedCategory))
            {
                settingsManager.LastDatabaseUpdate = DateTime.Now;

#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014

                Close(this);
            }
        }

        private void DeleteCategory()
        {
            if (categoryRepository.Delete(SelectedCategory))
            {
                settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014
            }
            Close(this);
        }

        private void Cancel()
        {
            SelectedCategory = categoryRepository.FindById(SelectedCategory.Id);
            Close(this);
        }
    }
}