using System;
using System.Threading.Tasks;
using MoneyManager.Foundation.Interfaces.Shotcuts;

namespace MoneyManager.Droid.Src.Widgets
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