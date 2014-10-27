using System.Reflection;

namespace MoneyManager.Business.Src
{
    public class Utilities
    {
        public static string GetVersion()
        {
            return Assembly.Load(new AssemblyName("MoneyManager.WindowsPhone")).FullName.Split('=')[1].Split(',')[0];
        }
    }
}