namespace MoneyFox.Ui.Common.Extensions;

using System.Globalization;
using System.Reflection;
using System.Resources;
using Resources.Strings;

[ContentProperty("Text")]
public class TranslateExtension : IMarkupExtension
{
    private static readonly Lazy<ResourceManager> resMgr = new(
        () => new(baseName: typeof(Translations).FullName, assembly: typeof(Translations).GetTypeInfo().Assembly));

    public string? Text { get; set; }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        if (Text == null)
        {
            return string.Empty;
        }

        return resMgr.Value.GetString(name: Text, culture: CultureInfo.CurrentUICulture) ?? Text;
    }
}
