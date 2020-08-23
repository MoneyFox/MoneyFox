using MoneyFox.Application;
using MoneyFox.Application.Resources;
using System;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Extensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
            () => new ResourceManager(typeof(Strings).FullName, typeof(Strings).GetTypeInfo().Assembly));

        public string? Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if(Text == null)
                return string.Empty;

            return ResMgr.Value.GetString(Text, CultureHelper.CurrentCulture) ?? Text;
        }
    }
}
