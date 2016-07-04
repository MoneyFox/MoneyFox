using PropertyChanged;

namespace MoneyFox.Shared.Helpers
{
    [ImplementPropertyChanged]
    public class GlobalBusyIndicatorState
    {
        public bool IsActive { get; set; } = false;
    }
}