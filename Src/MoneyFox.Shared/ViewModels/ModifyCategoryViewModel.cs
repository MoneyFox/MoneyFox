using System;
using System.Linq;
using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Interfaces.Repositories;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    /// <summary>
    ///     View Model for creating and editing Categories without dialog
    /// </summary>
    [ImplementPropertyChanged]
    public class ModifyCategoryViewModel : BaseViewModel {
        private readonly ICategoryRepository categoryRepository;
        private readonly IDialogService dialogService;

        public ModifyCategoryViewModel(ICategoryRepository categoryRepository, IDialogService dialogService) {
            this.categoryRepository = categoryRepository;
            this.dialogService = dialogService;
        }

        /// <summary>
        ///     Saves changes to a category if in edit mode <see cref="IsEdit" />  or creates
        ///     a new category.
        /// </summary>
        public MvxCommand SaveCommand => new MvxCommand(SaveCategory);

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public MvxCommand CancelCommand => new MvxCommand(Cancel);

        /// <summary>
        ///     Delete the selected category from the database
        /// </summary>
        public MvxCommand DeleteCommand => new MvxCommand(DeleteCategory);

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     The currently selected category
        /// </summary>
        public Category SelectedCategory { get; set; }

        /// <summary>
        ///     Indicates if the selected category shall be edited or a new one created.
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        ///     Returns the Title based on whether a category is being created or edited
        /// </summary>
        public string Title => IsEdit
            ? string.Format(Strings.EditCategoryTitle, SelectedCategory.Name)
            : Strings.AddCategoryTitle;

        /// <summary>
        ///     Initialize the ViewModel
        /// </summary>
        /// <param name="isEdit">Indicates if a category is being edited or created</param>
        public void Init(bool isEdit)
        {
            IsEdit = isEdit;

            if (!IsEdit)
            {
                SelectedCategory = new Category();
            }
        }

        /// <summary>
        ///     Initialize the ViewModel
        /// </summary>
        /// <param name="isEdit">Indicates if a category is being edited or created</param>
        /// <param name="selectedCategoryId">If we are editing a category this is its Id</param>
        public void Init(bool isEdit, int selectedCategoryId)
        {
            IsEdit = isEdit;
            SelectedCategory = selectedCategoryId != 0
                ? categoryRepository.GetList(x => x.Id == selectedCategoryId).First()
                : new Category();
        }

        private async void SaveCategory()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (!IsEdit && categoryRepository.GetList(a => string.Equals(a.Name, SelectedCategory.Name, StringComparison.CurrentCultureIgnoreCase))
                .Any())
            {
                await dialogService.ShowMessage(Strings.ErrorMessageSave, Strings.DuplicateCategoryMessage);
                return;
            }

            if (categoryRepository.Save(SelectedCategory))
            {
                SettingsHelper.LastDatabaseUpdate = DateTime.Now;
                Close(this);
            }
        }

        private void DeleteCategory()
        {
            if (categoryRepository.Delete(SelectedCategory))
                SettingsHelper.LastDatabaseUpdate = DateTime.Now;
            Close(this);
        }

        private void Cancel()
        {
            SelectedCategory = categoryRepository.FindById(SelectedCategory.Id);
            Close(this);
        }
    }
}