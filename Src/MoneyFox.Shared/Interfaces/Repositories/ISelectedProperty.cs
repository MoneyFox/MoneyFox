namespace MoneyFox.Shared.Interfaces.Repositories
{
    public interface ISelectedProperty<T>
    {
        /// <summary>
        ///     The selected Item
        /// </summary>
        T Selected { get; set; }
    }
}