using System;
using System.Globalization;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IBackupService backupService;
        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;

        public EditCategoryViewModel(int categoryId,
                                     IScreen hostScreen,
                                     ICrudServicesAsync crudServices = null,
                                     IDialogService dialogService = null,
                                     ISettingsFacade settingsFacade = null,
                                     IBackupService backupService = null)
            : base(categoryId, hostScreen, crudServices, settingsFacade, backupService, dialogService)
        {
            this.crudServices = crudServices ?? Locator.Current.GetService<ICrudServicesAsync>();
            this.dialogService = dialogService ?? Locator.Current.GetService<IDialogService>();
            this.settingsFacade = settingsFacade ?? Locator.Current.GetService<ISettingsFacade>();
            this.backupService = backupService ?? Locator.Current.GetService<IBackupService>();

            this.WhenActivated(async disposable =>
            {
                SelectedCategory = await this.crudServices.ReadSingleAsync<CategoryViewModel>(categoryId);
                Title = string.Format(CultureInfo.InvariantCulture, Strings.EditCategoryTitle, SelectedCategory.Name);

                DeleteCommand = ReactiveCommand.CreateFromTask(DeleteCategory).DisposeWith(disposable);
            });
        }

        public override string UrlPathSegment => "EditCategory";

        public override string Title { get; set; }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; set; }

        protected override async Task SaveCategory()
        {
            await crudServices.UpdateAndSaveAsync(SelectedCategory);
            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            await CancelCommand.Execute();
        }

        private async Task DeleteCategory()
        {
            await crudServices.DeleteAndSaveAsync<AccountViewModel>(SelectedCategory.Id);
            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
            await CancelCommand.Execute();
        }
    }
}