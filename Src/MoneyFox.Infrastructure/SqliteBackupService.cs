namespace MoneyFox.Infrastructure;

using System;
using System.IO;
using Core.Features;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Persistence;

internal sealed class SqliteBackupService : ISqliteBackupService
{
    private readonly AppDbContext context;

    public SqliteBackupService(AppDbContext context)
    {
        this.context = context;
    }

    public string CreateBackup()
    {
        var backupPath = Path.Combine(path1: Environment.GetFolderPath(Environment.SpecialFolder.Personal), path2: $"money-fox_{DateTime.UtcNow:yyyy-M-d_hh-mm-ssss}.backup");

        var dbConnection = new SqliteConnection(context.Database.GetConnectionString());
        var backup = new SqliteConnection($"Data Source={backupPath}");
        dbConnection.BackupDatabase(backup);

        return backupPath;
    }
}
