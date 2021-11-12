using System;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Migrations.Startup
{
    [ExcludeFromCodeCoverage]
    [SuppressMessage(
        "Major Code Smell",
        "S1118:Utility classes should not have public constructors",
        Justification = "Needed to Startup.")]
    internal class Program
    {
        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        private static void Main(string[] args) => Console.WriteLine("Migrating");
    }
}