using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyFox.Windows
{
    public class DefaultLanguageSetter
    {
        //Variable to store selected languge, setting default option in the Language settings combo box.
        public int DefaultLanguageIndex = 7;

        public void setIndex(int val)
        {
            DefaultLanguageIndex = val;
        }

        public int getIndex()
        {
            return DefaultLanguageIndex;
        }
    }
}
