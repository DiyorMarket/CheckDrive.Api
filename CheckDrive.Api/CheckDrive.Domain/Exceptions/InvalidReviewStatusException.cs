namespace CheckDrive.Domain.Exceptions;

public sealed class InvalidReviewStatusException : Exception
{
    public InvalidReviewStatusException() : base() { }
    public InvalidReviewStatusException(string message) : base(message) { }
}
