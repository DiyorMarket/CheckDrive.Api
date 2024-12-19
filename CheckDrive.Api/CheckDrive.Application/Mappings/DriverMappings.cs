using AutoMapper;
using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.Mappings;

internal sealed class DriverMappings : Profile
{
    public DriverMappings()
    {
        CreateMap<Driver, DriverDto>()
            .ForCtorParam(nameof(DriverDto.FullName), cfg => cfg.MapFrom(e => $"{e.FirstName} {e.Patronymic} {e.LastName}"));

        CreateMap<CheckPoint, DriverHistoryDto>()
            .ForCtorParam(nameof(DriverHistoryDto.TravelledDistance), cfg => cfg.MapFrom(e => e.ManagerReview!.FinalMileage - e.ManagerReview.InitialMileage))
            .ForCtorParam(nameof(DriverHistoryDto.FuelConsumptionAmount), cfg => cfg.MapFrom(e => e.ManagerReview!.FuelConsumptionAmount))
            .ForCtorParam(nameof(DriverHistoryDto.DebtAmount), cfg => cfg.MapFrom(e => e.ManagerReview!.DebtAmount))
            .ForCtorParam(nameof(DriverHistoryDto.IsCompletedSuccessfully), cfg => cfg.MapFrom(e => e.Status == CheckPointStatus.Completed))
            .ForCtorParam(nameof(DriverHistoryDto.StartDate), cfg => cfg.MapFrom(e => e.StartDate))
            .ForCtorParam(nameof(DriverHistoryDto.Car), cfg => cfg.MapFrom(e => e.MechanicHandover!.Car));
    }
}
