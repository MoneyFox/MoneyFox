using System;
using System.Threading.Tasks;

namespace MoneyFox.Core.Interfaces
{
    public interface IBrowserAdapter
    {
        Task OpenWebsiteAsync(Uri uri);
    }
}