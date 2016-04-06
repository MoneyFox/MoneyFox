using System;
using System.Threading.Tasks;
using MoneyFox.Shared.Interfaces.Shotcuts;

namespace MoneyFox.Droid.Widgets
{
    public class IncomeWidget : IIncomeShortcut
    {
        public Task CreateShortCut()
        {
            throw new NotImplementedException();
        }

        public Task RemoveShortcut()
        {
            throw new NotImplementedException();
        }

        public bool IsShortcutExisting
        {
            get { throw new NotImplementedException(); }
        }
    }
}