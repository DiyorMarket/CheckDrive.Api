namespace CheckDrive.Application.Interfaces.Jobs;

public interface IResetDriverStatusService
{
    Task ExecuteDailyResetAsync();
}
