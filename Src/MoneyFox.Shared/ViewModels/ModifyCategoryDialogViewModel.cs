using MoneyFox.Shared.Helpers;
using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Resources;
using MvvmCross.Core.ViewModels;
using PropertyChanged;
using System;
using System.Linq;

namespace MoneyFox.Shared.ViewModels {
    /// <summary>
    ///     View Model for creating and editing Categories
    /// </summary>
    [ImplementPropertyChanged]
    public class ModifyCategoryDialogViewModel : BaseViewModel {
        private readonly IRepository<Category> categoryRepository;
        private readonly IDialogService dialogService;

        /// <summary>
        ///     Create new instance of the view model.
        /// </summary>
        /// <param name="categoryRepository">Instance of the category repository to access the db.</param>
        /// <param name="dialogService">Dialogservice to interact with the user.</param>
        public ModifyCategoryDialogViewModel(IRepository<Category> categoryRepository, IDialogService dialogService) {
            this.categoryRepository = categoryRepository;
            this.dialogService = dialogService;
        }

        public Category Selected { get; set; }

        /// <summary>
        ///     Indicates if the selected category shall be edited or a new one created.
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        ///     Prepares the view when it is loaded.
        /// </summary>
        public IMvxCommand LoadedCommand => new MvxCommand(Loaded);

        /// <summary>
        ///     Completes the Operation when all information was entered.
        /// </summary>
        public IMvxCommand DoneCommand => new MvxCommand(Done);

        /// <summary>
        ///  Returns the Title depending on if the view is in edit mode or not.
        /// </summary>
        public string Title => IsEdit ? Strings.EditCategoryTitle : Strings.AddCategoryTitle;

        private void Loaded() {
            if (IsEdit) {
                return;
            }

            Selected = new Category();
        }

        private async void Done() {
            if (string.IsNullOrEmpty(Selected.Name)) {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (categoryRepository.Data.Any(x => string.Equals(x.Name, Selected.Name, StringComparison.CurrentCultureIgnoreCase))) {
                await dialogService.ShowMessage(Strings.ErrorMessageSave, Strings.DuplicateCategoryMessage);
                return;
            }

            if (categoryRepository.Save(Selected)) {
                SettingsHelper.LastDatabaseUpdate = DateTime.Now;
            }
        }
    }
}