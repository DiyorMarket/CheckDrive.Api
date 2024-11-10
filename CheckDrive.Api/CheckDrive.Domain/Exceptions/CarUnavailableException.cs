namespace CheckDrive.Domain.Exceptions;

public sealed class CarUnavailableException : Exception
{
    public CarUnavailableException() : base() { }
    public CarUnavailableException(string message) : base(message) { }
}
