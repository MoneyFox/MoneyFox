using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using MoneyFox.ViewModels.Accounts;
using MoneyFox.ViewModels.Dashboard;

namespace MoneyFox
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            if(!ServiceLocator.IsLocationProviderSet && ViewModelBase.IsInDesignModeStatic)
            {
                RegisterServices(new ContainerBuilder());
            }
        }

        public static void RegisterServices(ContainerBuilder registrations)
        {
            IContainer container = registrations.Build();

            if(container != null)
            {
                ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            }
        }

        // ViewModels
        public static DashboardViewModel DashboardViewModel => ServiceLocator.Current.GetInstance<DashboardViewModel>();
        public static AccountListViewModel AccountListViewModel => ServiceLocator.Current.GetInstance<AccountListViewModel>();

    }
}
