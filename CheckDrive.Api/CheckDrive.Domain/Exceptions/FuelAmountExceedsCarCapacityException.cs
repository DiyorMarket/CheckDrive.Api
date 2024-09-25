namespace CheckDrive.Domain.Exceptions;

public sealed class FuelAmountExceedsCarCapacityException : Exception
{
    public FuelAmountExceedsCarCapacityException() : base() { }
    public FuelAmountExceedsCarCapacityException(string message) : base(message) { }
}
