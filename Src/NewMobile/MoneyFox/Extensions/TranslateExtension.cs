using MoneyFox.Application.Resources;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Extensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private readonly CultureInfo ci;

        static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
            () => new ResourceManager(typeof(Strings).FullName, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public string Text { get; set; }

        public TranslateExtension()
        {
            if(Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                ci = CultureInfo.CurrentCulture;
            }
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if(Text == null)
                return string.Empty;

            return ResMgr.Value.GetString(Text, ci) ?? Text;
        }
    }
}
