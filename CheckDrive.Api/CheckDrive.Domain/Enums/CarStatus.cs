namespace CheckDrive.Domain.Enums;

public enum CarStatus
{
    Free,               // Car is free and ready for use
    Busy,               // Car is currently assigned to a driver for a ride and being used
    LimitReached,       // Car has reached its monthly or yearly limits
    OutOfService        // Car is broken or has unresolved issues
}
