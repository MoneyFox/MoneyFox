namespace MoneyFox.Core.ApplicationCore.Domain.Exceptions;

using System;

public class NoBackupFoundException : Exception
{
    public NoBackupFoundException() : base("No backup with was found") { }
}
