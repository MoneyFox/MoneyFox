using Microsoft.Graph;

namespace MoneyFox.Foundation
{
	public interface ICustomAuthenticationProvider : IAuthenticationProvider
	{
		void Logout();
	}
}
