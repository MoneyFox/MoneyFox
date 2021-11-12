using System;
using System.Threading.Tasks;

namespace MoneyFox.Application.Common.Adapters
{
    public interface IBrowserAdapter
    {
        Task OpenWebsiteAsync(Uri uri);
    }
}