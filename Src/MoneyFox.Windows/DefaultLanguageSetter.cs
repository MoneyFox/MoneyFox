using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyFox.Windows
{
    public class DefaultLanguageSetter
    {
        //boolean flag used to check if a language has been selected previously
        public static bool LangSelected = false;
        //Variable to store selected languge, setting default option in the Language settings combo box.
        public int DefaultLanguageIndex = 7;

        //Variable to store chosen language code
        public static string ChosenLang;

        public void setIndex(int val)
        {
            DefaultLanguageIndex = val;
        }

        public int getIndex()
        {
            return DefaultLanguageIndex;
        }

        //Depending on the Langage of the machine, the default language index for the combo box is set.
    //    if (shell.Language.Contains("de"))
    //        DLS.setIndex(0);
    //    if (shell.Language.Contains("de"))
    //        DLS.setIndex(1);
    //    if (shell.Language.Contains("es"))
    //        DLS.setIndex(2);
    //    if (shell.Language.Contains("pt"))
    //        DLS.setIndex(3);
    //   if (shell.Language.Contains("ru"))
    //        DLS.setIndex(4);
    //   if (shell.Language.Contains("cn"))
    //        DLS.setIndex(5);
    }
}
