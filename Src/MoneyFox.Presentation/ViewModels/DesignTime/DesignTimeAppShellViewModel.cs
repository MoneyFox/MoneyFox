using MoneyFox.Application.Resources;
using System.Globalization;
using MoneyFox.Presentation.Utilities;

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
