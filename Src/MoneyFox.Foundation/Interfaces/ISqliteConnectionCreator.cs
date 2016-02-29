namespace MoneyManager.Foundation.Interfaces
{
    public interface ISqliteConnectionCreator
    {
        SQLiteConnection GetConnection();
    }
}