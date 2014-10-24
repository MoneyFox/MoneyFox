using System.Reflection;

namespace MoneyManager.Business
{
    internal class Utilities
    {
        public static string GetVersion()
        {
            return Assembly.Load(new AssemblyName("MoneyManager.WindowsPhone")).FullName.Split('=')[1].Split(',')[0];
        }
    }
}