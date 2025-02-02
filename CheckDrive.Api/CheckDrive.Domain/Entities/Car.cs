using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

public class Car : EntityBase
{
    public required string Model { get; set; }
    public required string Number { get; set; }
    public int ManufacturedYear { get; set; }
    public decimal Mileage { get; set; }
    public decimal FuelCapacity { get; set; }
    public decimal AverageFuelConsumption { get; set; }
    public decimal RemainingFuel { get; set; }
    public CarStatus Status { get; set; }

    public required CarLimits Limits { get; set; }
    public required CarUsageSummary UsageSummary { get; set; }

    public int? OilMarkId { get; set; }
    public virtual OilMark? OilMark { get; set; }

    public virtual ICollection<MechanicHandover> Handovers { get; set; }
    public virtual ICollection<Driver> AssignedDrivers { get; set; }

    public Car()
    {
        Handovers = [];
    }
}
