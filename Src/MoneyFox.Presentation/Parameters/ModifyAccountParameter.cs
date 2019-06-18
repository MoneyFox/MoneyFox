namespace MoneyFox.Presentation.Parameters
{
    /// <summary>
    ///     Parameter object for the ModifyAccountView.
    /// </summary>
    public class ModifyAccountParameter
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="accountId">Account Id to edit</param>
        public ModifyAccountParameter(int accountId = 0)
        {
            AccountId = accountId;
        }

        /// <summary>
        ///     AccountId who shall be edited.
        ///     If this is 0, a new account shall be created.
        /// </summary>
        public int AccountId { get; set; }
    }
}
