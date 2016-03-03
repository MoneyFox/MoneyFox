using System.Linq;
using System.Reflection;
using Cimbalino.Toolkit.Services;
using Microsoft.Practices.ServiceLocation;
using MoneyFox.Core.Repositories;
using SimpleInjector;

namespace MoneyManager.Windows
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var container = new Container();

            ServiceLocator.SetLocatorProvider(() => new SimpleInjectorServiceLocatorAdapter(container));

            container.Register<IEmailComposeService>(() => new EmailComposeService());
            container.Register<IStoreService>(() => new StoreService());

            var dataAccessAssembly = typeof (GenericDataRepository<>).GetTypeInfo().Assembly;

            var registrations =
                from type in dataAccessAssembly.GetExportedTypes()
                where type.Namespace == "MoneyManager.DataAccess"
                where type.GetInterfaces().Any()
                select new {Service = type.GetInterfaces().Single(), Implementation = type};

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation, Lifestyle.Transient);
            }
        }
    }

    public class NavigationConstants
    {
        
    }
}