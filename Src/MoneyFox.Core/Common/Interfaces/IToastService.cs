namespace MoneyFox.Core.Common.Interfaces;

using System.Threading.Tasks;

public interface IToastService
{
    Task ShowToastAsync(string message, string title = "");
}
