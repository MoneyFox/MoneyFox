namespace MoneyManager.Foundation.OperationContracts
{
    public interface IUserNotification {
        
        /// <summary>
        /// Sets the MainTile with new Information
        /// </summary>
        /// <param name="income">Income of these month</param>
        /// <param name="spending">Spending of these month</param>
        /// <param name="earnings">Earnings of these month </param>
        void UpdateMainTile(double income, double spending, double earnings);
    }
}
