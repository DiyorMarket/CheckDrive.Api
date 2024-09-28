namespace CheckDrive.Domain.Exceptions;

public sealed class UnavailableCarException : Exception
{
    public UnavailableCarException() : base() { }
    public UnavailableCarException(string message) : base(message) { }
}
