﻿using System.Globalization;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected ViewModelBase() {
            Resources = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);
        }

        /// <summary>
        ///     Provides Access to the LocalizedResources for the current language
        /// </summary>
        public LocalizedResources Resources { get; }
    }
}
