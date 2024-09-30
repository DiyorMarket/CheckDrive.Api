namespace CheckDrive.Domain.Exceptions;
public class InvalidLoginAttemptException : Exception
{

    public InvalidLoginAttemptException() : base() { }

    public InvalidLoginAttemptException(string message) : base(message) { }

    public InvalidLoginAttemptException(string message, Exception inner) : base(message, inner) { }
}
