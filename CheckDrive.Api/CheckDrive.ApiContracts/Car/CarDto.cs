namespace CheckDrive.ApiContracts.Car
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int Mileage { get; set; }
        public int OneYearMediumDistance { get; set; }
        public int OneMonthMediumDistance { get; set; }
        public int OneYearMeduimFuelConsumption { get; set; }
        public int OneMonthMeduimFuelConsumption { get; set; }
        public double MeduimFuelConsumption { get; set; }
        public double FuelTankCapacity { get; set; }
        public double RemainingFuel {  get; set; }
        public int ManufacturedYear { get; set; }
    }
}
