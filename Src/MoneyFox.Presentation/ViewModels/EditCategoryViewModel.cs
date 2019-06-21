using System;
using System.Globalization;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.Services;
using MoneyFox.ServiceLayer.Facades;
using NLog;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly ICrudServicesAsync crudServices;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupService backupService;

        public EditCategoryViewModel(ICrudServicesAsync crudServices,
            IDialogService dialogService,
            ISettingsFacade settingsFacade,
            IBackupService backupService,
            INavigationService navigationService)
            : base(crudServices, settingsFacade, backupService, navigationService)
        {
            this.crudServices = crudServices;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;

        }

        public RelayCommand InitializeCommand => new RelayCommand(Initialize);

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public RelayCommand DeleteCommand => new RelayCommand(DeleteCategory);

        private async void Initialize()
        {
            SelectedCategory = await crudServices.ReadSingleAsync<CategoryViewModel>(CategoryId);
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditCategoryTitle, SelectedCategory.Name);
        }

        protected override async Task SaveCategory()
        {
            await crudServices.UpdateAndSaveAsync(SelectedCategory);
            if (!crudServices.IsValid)
            {
                await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            }

            CancelCommand.Execute(null);
        }

        private async void DeleteCategory()
        {
            await crudServices.DeleteAndSaveAsync<Category>(SelectedCategory.Id);

            logManager.Info("Category with Id {id} deleted.", SelectedCategory.Id);

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
#pragma warning disable 4014
            backupService.EnqueueBackupTask();
#pragma warning restore 4014
            CancelCommand.Execute(null);
        }
    }
}