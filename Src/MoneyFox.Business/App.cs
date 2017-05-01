using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business
{
    /// <summary>
    ///     Entry piont to the Application for MvvmCross.
    /// </summary>
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