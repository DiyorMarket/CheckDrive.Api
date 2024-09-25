namespace CheckDrive.Domain.Exceptions;

public sealed class FuelRefilExceededException : Exception
{
    public FuelRefilExceededException() : base() { }
    public FuelRefilExceededException(string message) : base(message) { }
}
