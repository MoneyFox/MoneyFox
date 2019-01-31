using Microsoft.Graph;

namespace MoneyFox.ServiceLayer.Interfaces
{
    /// <summary>
    ///     Provides an interface for a custom authentication provider
    /// </summary>
    public interface ICustomAuthenticationProvider : IAuthenticationProvider
	{
        /// <summary>
        ///     Log out, delete cached information and close the session.
        /// </summary>
        void Logout();
	}
}
