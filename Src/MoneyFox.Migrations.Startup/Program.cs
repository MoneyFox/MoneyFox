using System;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Migrations.Startup
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "RunTimeComponent")]
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Migrating");
        }
    }
}
