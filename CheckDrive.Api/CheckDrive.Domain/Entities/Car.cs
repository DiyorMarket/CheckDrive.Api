using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

// TODO: Add proper validation for the car with new properties introduced,
// such as monthly, yearly distance and fuel consumption limits
public class Car : EntityBase
{
    public string Model { get; set; }
    public string Color { get; set; }
    public string Number { get; set; }
    public int ManufacturedYear { get; set; }
    public int Mileage { get; set; }
    public int CurrentMonthMileage { get; set; }
    public int CurrentYearMileage { get; set; }
    public int MonthlyDistanceLimit { get; set; }
    public int YearlyDistanceLimit { get; set; }
    public decimal CurrentMonthFuelConsumption { get; set; }
    public decimal CurrentYearFuelConsumption { get; set; }
    public decimal MonthlyFuelConsumptionLimit { get; set; }
    public decimal YearlyFuelConsumptionLimit { get; set; }
    public decimal AverageFuelConsumption { get; set; }
    public decimal FuelCapacity { get; set; }
    public decimal RemainingFuel { get; set; }
    public CarStatus Status { get; set; }

    public virtual ICollection<MechanicHandover> Handovers { get; set; }

    public Car()
    {
        Handovers = new HashSet<MechanicHandover>();
    }
}
