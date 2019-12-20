using System.Globalization;
using GalaSoft.MvvmLight;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Utilities;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IBaseViewModel
    {
    }

    public abstract class BaseViewModel : ViewModelBase, IBaseViewModel
    {
    }
}
