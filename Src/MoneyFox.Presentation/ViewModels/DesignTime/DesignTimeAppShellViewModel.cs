using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using System.Globalization;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeAppShell
    {
        public DesignTimeAppShell()
        {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <inheritdoc />
        public LocalizedResources Resources { get; }
    }
}
