namespace MoneyFox.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IBrowserAdapter
    {
        Task OpenWebsiteAsync(Uri uri);
    }
}