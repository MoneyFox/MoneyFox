using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using CommonServiceLocator;
using MoneyFox.Uwp.Services;

namespace MoneyFox.Uwp.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        private readonly string navElement;

        public NavigationService NavigationService => ServiceLocator.Current.GetInstance<NavigationService>();

        public DefaultLaunchActivationHandler(Type navElement)
        {
            this.navElement = navElement.Name;
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter
            NavigationService.NavigateTo(navElement, args.Arguments);

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return NavigationService.Frame.Content == null && navElement != null;
        }
    }
}
