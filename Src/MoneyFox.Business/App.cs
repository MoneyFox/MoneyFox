using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            // Start the app.
            RegisterAppStart(new AppStart());
        }
    }
}