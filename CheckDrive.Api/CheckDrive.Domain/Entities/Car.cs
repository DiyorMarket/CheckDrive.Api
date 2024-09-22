using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

public class Car : EntityBase
{
    public string Model { get; set; }
    public string Color { get; set; }
    public string Number { get; set; }
    public int ManufacturedYear { get; set; }
    public int Mileage { get; set; }
    public int YearlyDistanceLimit { get; set; }
    public decimal AverageFuelConsumption { get; set; }
    public decimal FuelCapacity { get; set; }
    public decimal RemainingFuel { get; set; }
    public CarStatus Status { get; set; }

    public virtual ICollection<CheckPoint> CheckPoints { get; set; }

    public Car()
    {
        CheckPoints = new HashSet<CheckPoint>();
    }
}
