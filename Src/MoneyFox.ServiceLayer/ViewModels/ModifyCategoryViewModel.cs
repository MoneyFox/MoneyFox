using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Facades;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Services;
using ReactiveUI;
using Splat;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public interface IModifyCategoryViewModel
    {
        /// <summary>
        ///     Saves changes to a CategoryViewModel if in edit mode <see cref="IsEdit" />  or creates
        ///     a new CategoryViewModel.
        /// </summary>
        ReactiveCommand<Unit, Unit> SaveCommand { get; }

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        ReactiveCommand<Unit, Unit> CancelCommand { get; }

        /// <summary>
        ///     Selected category.
        /// </summary>
        CategoryViewModel SelectedCategory { get; }
    }

    /// <summary>
    ///     View Model for creating and editing Categories without dialog
    /// </summary>
    public abstract class ModifyCategoryViewModel : RouteableViewModelBase, IModifyCategoryViewModel
    {
        private readonly ICrudServicesAsync crudServices;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupService backupService;
        private readonly IDialogService dialogService;

        private CategoryViewModel selectedCategory;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected ModifyCategoryViewModel(int categoryId,
                                          IScreen hostScreen,
                                          ICrudServicesAsync crudServices = null,
                                          ISettingsFacade settingsFacade = null,
                                          IBackupService backupService = null,
                                          IDialogService dialogService = null)
        {
            CategoryId = categoryId;
            HostScreen = hostScreen;
            this.crudServices = crudServices ?? Locator.Current.GetService<ICrudServicesAsync>();
            this.settingsFacade = settingsFacade ?? Locator.Current.GetService<ISettingsFacade>();
            this.backupService = backupService ?? Locator.Current.GetService<IBackupService>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();

            this.WhenActivated(disposable =>
            {
                SaveCommand = ReactiveCommand.CreateFromTask(SaveCategoryBase).DisposeWith(disposable);
                CancelCommand = ReactiveCommand.CreateFromTask(Cancel).DisposeWith(disposable);

            });
        }

        public override IScreen HostScreen { get; }

        /// <summary>
        ///     The currently selected CategoryViewModel
        /// </summary>
        public CategoryViewModel SelectedCategory {
            get => selectedCategory;
            set => this.RaiseAndSetIfChanged(ref selectedCategory, value);
        }

        /// <summary>
        ///     Returns the Title based on whether a CategoryViewModel is being created or edited
        /// </summary>
        public virtual string Title { get; set; }

        protected int CategoryId { get; private set; }

        /// <summary>
        ///     Saves changes to a CategoryViewModel if in edit mode <see cref="IsEdit" />  or creates
        ///     a new CategoryViewModel.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SaveCommand { get; set; }

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCommand { get; set; }

        protected abstract Task SaveCategory();

        private async Task SaveCategoryBase()
        {
            if (string.IsNullOrEmpty(SelectedCategory.Name)) {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            await SaveCategory();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
        }
        
        private async Task Cancel()
        {
            SelectedCategory = await crudServices.ReadSingleAsync<CategoryViewModel>(SelectedCategory.Id);
            HostScreen.Router.NavigateBack.Execute();
        }
    }
}
