namespace CheckDrive.Application.Interfaces.Jobs;

public interface IResetCarLimitsService
{
    Task ExecuteMonthlyResetAsync();
    Task ExecuteYearlyResetAsync();
}
