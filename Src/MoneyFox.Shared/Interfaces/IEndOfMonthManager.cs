using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyFox.Shared.Interfaces
{
    public interface IEndOfMonthManager
    {
        string Hope();
        string DetermineEnd(int x, double y);
    }
}
