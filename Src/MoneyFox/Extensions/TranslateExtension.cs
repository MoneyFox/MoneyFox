namespace MoneyFox.Extensions
{
    using Core._Pending_;
    using Core.Resources;
    using System;
    using System.Reflection;
    using System.Resources;
    using Core.Common;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
            () => new ResourceManager(typeof(Strings).FullName, typeof(Strings).GetTypeInfo().Assembly));

        public string? Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if(Text == null)
            {
                return string.Empty;
            }

            return ResMgr.Value.GetString(Text, CultureHelper.CurrentCulture) ?? Text;
        }
    }
}