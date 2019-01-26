using System.Linq;
using GenericServices;
using GenericServices.PublicButHidden;
using GenericServices.Setup;
using Microsoft.EntityFrameworkCore;
using MoneyFox.BusinessDbAccess.StatisticDataProvider;
using MoneyFox.BusinessLogic.StatisticDataProvider;
using MoneyFox.DataLayer;
using MoneyFox.Foundation;
using MoneyFox.ServiceLayer.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace MoneyFox.Presentation
{
    /// <summary>
    ///     Entry point to the Application for MvvmCross.
    /// </summary>
    public class 
        CoreApp : MvxApplication
    {
        public static AppPlatform CurrentPlatform { get; set; }

        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            //Mvx.IoCProvider.ConstructAndRegisterSingleton<IPasswordStorage, PasswordStorage>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ICrudServices, CrudServices>();

            //typeof(OneDriveService).Assembly.CreatableTypes()
            //                      .EndingWith("Service")
            //                      .AsInterfaces()
            //                      .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Adapter")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Manager")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            //Mvx.IoCProvider.RegisterType(() => new Session(Mvx.IoCProvider.Resolve<ISettingsManager>()));

            typeof(IStatisticDbAccess).Assembly.CreatableTypes()
                                 .EndingWith("DbAccess")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            typeof(CashFlowDataProvider).Assembly.CreatableTypes()
                                 .EndingWith("Provider")
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("Factory")
                                 .AsInterfaces()
                                 .RegisterAsLazySingleton();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("DataProvider")
                                 .AsInterfaces()
                                 .RegisterAsLazySingleton();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("ViewModel")
                                 .Where(x => !x.Name.StartsWith("DesignTime"))
                                 .AsTypes()
                                 .RegisterAsDynamic();

            typeof(MainViewModel).Assembly.CreatableTypes()
                                 .EndingWith("ViewModel")
                                 .Where(x => !x.Name.StartsWith("DesignTime"))
                                 .AsInterfaces()
                                 .RegisterAsDynamic();

            SetupContextAndCrudServices();
            RegisterAppStart<MainViewModel>();

            //if (!Mvx.IoCProvider.CanResolve<Session>()) return;

            //if (Mvx.IoCProvider.Resolve<Session>().ValidateSession())
            //{
            //    if (CurrentPlatform == AppPlatform.UWP)
            //    {
            //        RegisterAppStart<AccountListViewModel>();
            //    } else
            //    {
            //        RegisterAppStart<MainViewModel>();
            //    }
            //} else
            //{
            //    RegisterAppStart<LoginViewModel>();
            //}
        }

        private void SetupContextAndCrudServices()
        {
            var context = SetupEfContext();

            Mvx.IoCProvider.RegisterType<EfCoreContext>(SetupEfContext);
            Mvx.IoCProvider.RegisterType<ICrudServicesAsync>(() => SetUpCrudServices(context));
        }

        private EfCoreContext SetupEfContext()
        {
            var context = new EfCoreContext();
            context.Database.Migrate();

            return context;
        }

        private ICrudServicesAsync SetUpCrudServices(EfCoreContext context)
        {
            var utData = context.SetupSingleDtoAndEntities<AccountViewModel>();
            utData.AddSingleDto<CategoryViewModel>();
            utData.AddSingleDto<PaymentViewModel>();
            utData.AddSingleDto<RecurringPaymentViewModel>();

            return new CrudServicesAsync(context, utData.ConfigAndMapper);
        }
    }
}