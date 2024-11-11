using CheckDrive.Domain.Entities;
using CsvHelper.Configuration;

namespace CheckDrive.Application.Mappings.CSV;

public sealed class CarCsvMappings : ClassMap<Car>
{
    public CarCsvMappings()
    {
        Map(c => c.Model).Index(0);
        Map(c => c.Number).Index(1);
        Map(c => c.ManufacturedYear).Index(2);
        Map(c => c.AverageFuelConsumption).Index(4);
        Map(c => c.MonthlyDistanceLimit).Index(5);
        Map(c => c.MonthlyFuelConsumptionLimit).Index(6);
        Map(c => c.YearlyDistanceLimit).Index(7);
        Map(c => c.YearlyFuelConsumptionLimit).Index(8);
    }
}
