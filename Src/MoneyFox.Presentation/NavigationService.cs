using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MoneyFox.Application.Common.Interfaces;
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
                    if (!Navigation.NavigationStack.Any()) return null;

                    Type pageType = Navigation.NavigationStack.First().GetType();

                    return PagesByKey.ContainsValue(pageType)
                        ? PagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public static void Configure(string pageKey, Type pageType)
        {
            lock (PagesByKey)
            {
                if (PagesByKey.ContainsKey(pageKey))
                    PagesByKey[pageKey] = pageType;
                else
                    PagesByKey.Add(pageKey, pageType);
            }
        }

        public static void Initialize(INavigation navigation)
        {
            Navigation = navigation;
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
                    ConstructorInfo constructor;
                    object[] parameters;
                    GetConstructor(pageKey, parameter, out constructor, out parameters);

                    var page = constructor.Invoke(parameters) as Page;
                    Navigation.PushAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                                                $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
                                                nameof(pageKey));
                }
            }
        }

        public void NavigateToModal(string pageKey)
        {
            NavigateToModal(pageKey, null);
        }

        public void NavigateToModal(string pageKey, object parameter)
        {
            lock (PagesByKey)
            {
                if (PagesByKey.ContainsKey(pageKey))
                {
                    ConstructorInfo constructor;
                    object[] parameters;
                    GetConstructor(pageKey, parameter, out constructor, out parameters);

                    var page = constructor.Invoke(parameters) as Page;
                    Navigation.PushModalAsync(page);
                }
                else
                {
                    throw new ArgumentException(
                                                $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?",
                                                nameof(pageKey));
                }
            }
        }

        private static void GetConstructor(string pageKey, object parameter, out ConstructorInfo constructor, out object[] parameters)
        {
            Type type = PagesByKey[pageKey];

            if (parameter == null)
            {
                constructor = type.GetTypeInfo()
                                  .DeclaredConstructors
                                  .FirstOrDefault(c => !c.GetParameters().Any());

                parameters = new object[]
                    { };
            }
            else
            {
                constructor = type.GetTypeInfo()
                                  .DeclaredConstructors
                                  .FirstOrDefault(
                                                  c =>
                                                  {
                                                      ParameterInfo[] p = c.GetParameters();

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
        }

        public void GoBack()
        {
            Navigation.PopAsync();
        }

        public void GoBackModal()
        {
            Navigation.PopModalAsync();
        }
    }
}
