using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using SimpleInjector;

namespace MoneyManager.Windows
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var container = new Container();

            ServiceLocator.SetLocatorProvider(() => new SimpleInjectorServiceLocatorAdapter(container));

            var dataAccessAssembly = typeof(AccountDataAccess).GetTypeInfo().Assembly;

            var registrations =
                from type in dataAccessAssembly.GetExportedTypes()
                where type.Namespace == "MoneyManager.DataAccess"
                where type.GetInterfaces().Any()
                select new { Service = type.GetInterfaces().Single(), Implementation = type };

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation, Lifestyle.Transient);
            }
        }
    }
}
