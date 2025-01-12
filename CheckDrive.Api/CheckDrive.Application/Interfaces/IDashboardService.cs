using CheckDrive.Application.DTOs.Dashboard;

namespace CheckDrive.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}
