namespace CheckDrive.Domain.Exceptions;

public sealed class InvalidCheckPointStageException : Exception
{
    public InvalidCheckPointStageException() : base() { }
    public InvalidCheckPointStageException(string message) : base(message) { }
}
