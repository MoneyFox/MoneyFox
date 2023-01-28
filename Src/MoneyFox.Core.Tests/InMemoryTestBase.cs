namespace MoneyFox.Core.Tests;

using Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public abstract class InMemoryTestBase : IDisposable
{
    private readonly SqliteConnection connection;

    protected InMemoryTestBase()
    {
        connection = new("Filename=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        Context = new(options: options, publisher: null!, settingsFacade: null!);
        Context.Database.EnsureCreated();
    }

    protected AppDbContext Context { get; }

    public void Dispose()
    {
        connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
