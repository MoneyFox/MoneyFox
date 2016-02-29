using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Core.ViewModels;

namespace MoneyManager.Core
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register(CreateNavigationService);

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<ModifyMedicineViewModel>();
            SimpleIoc.Default.Register<MedicineListViewModel>();

            SimpleIoc.Default.Register<AboutViewModel>();

            SimpleIoc.Default.Register<IGenericDataRepository<Medicine>, GenericDataRepository<Medicine>>();
            SimpleIoc.Default.Register<IGenericDataRepository<IntakeReason>, GenericDataRepository<IntakeReason>>();
            SimpleIoc.Default.Register<IGenericDataRepository<IntakeReminder>, GenericDataRepository<IntakeReminder>>();
            SimpleIoc.Default.Register<IGenericDataRepository<MedicineType>, GenericDataRepository<MedicineType>>();
        }

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new PageNavigationService();
            navigationService.Configure(NavigationConstants.MODIFY_MEDICINE_VIEW, typeof(ModifyMedicineView));

            return navigationService;
        }
    }
}
