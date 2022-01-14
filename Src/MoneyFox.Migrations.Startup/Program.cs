using System;
using System.Diagnostics.CodeAnalysis;

namespace MoneyFox.Migrations.Startup
{
    [ExcludeFromCodeCoverage]

    internal class Program
    {
        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        private static void Main(string[] args) => Console.WriteLine("Migrating");
    }
}