namespace CheckDrive.ApiContracts.Car
{
    public class CarHistoryDto
    {
        public string Model { get; set; }
        public string Number { get; set; }

        public double MonthlyNormalOilSpend { get; set; }
        public double MonthlyRefueledOil { get; set; }
        public double MonthlySpentOil { get; set; }
        public double RemainingFuel { get; set; }

        public int MonthlyMediumDistance { get; set; }
        public int MonthlyMileage { get; set; }
    }
}
