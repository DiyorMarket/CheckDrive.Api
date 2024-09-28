namespace CheckDrive.Domain.Exceptions;

public sealed class InvalidMileageException : Exception
{
    public InvalidMileageException() : base() { }
    public InvalidMileageException(string message) : base(message) { }
}
