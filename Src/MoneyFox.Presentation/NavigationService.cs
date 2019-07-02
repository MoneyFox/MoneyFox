using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

namespace MoneyFox.Presentation
{
    public class NavigationService : INavigationService
    {
        private static readonly Dictionary<string, Type> PagesByKey = new Dictionary<string, Type>();
        private static INavigation Navigation;

        public string CurrentPageKey
        {
            get
            {
                lock (PagesByKey)
                {
                    if (!Navigation.NavigationStack.Any())
                    {
                        return null;
                    }

                    var pageType = Navigation.NavigationStack.First().GetType();

                    return PagesByKey.ContainsValue(pageType)
                        ? PagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public void GoBack()
        {
            Navigation.PopAsync();
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            lock (PagesByKey)
            {
                if (PagesByKey.ContainsKey(pageKey))
                {
                    var type = PagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;

                    if (parameter == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());

                        parameters = new object[]
                        {
                        };
                    } else
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(
                                c =>
                                {
                                    var p = c.GetParameters();
                                    return p.Count() == 1
                                           && p[0].ParameterType == parameter.GetType();
                                });

                        parameters = new[]
                        {
                        parameter
                    };
                    }

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + pageKey);
                    }

                    var page = constructor.Invoke(parameters) as Page;
                    Navigation.PushAsync(page);
                } else
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }
            }
        }

        public static void Configure(string pageKey, Type pageType)
        {
            lock (PagesByKey)
            {
                if (PagesByKey.ContainsKey(pageKey))
                {
                    PagesByKey[pageKey] = pageType;
                } else
                {
                    PagesByKey.Add(pageKey, pageType);
                }
            }
        }

        public static void Initialize(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
