namespace MoneyManager.Foundation.OperationContracts
{
    public interface IDatabasePath
    {
        /// <summary>
        ///     Path to the db folder. Databasename has not to be added.
        ///     The DbHelper class will do this to ensure it's only at one place.
        /// </summary>
        string DbPath { get; }
    }
}