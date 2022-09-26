namespace MoneyFox.Core.ApplicationCore.Domain.Exceptions;

using System;

[Serializable]
public class NoBackupFoundException : Exception
{
    public NoBackupFoundException() : base("No backup with was found") { }
}

