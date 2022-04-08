namespace MoneyFox.Core.Common.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class NoBackupFoundException : Exception
    {
        public NoBackupFoundException(): base("No backup with was found")
        {
        }
    }
}
