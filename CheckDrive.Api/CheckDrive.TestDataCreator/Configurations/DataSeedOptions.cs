namespace CheckDrive.TestDataCreator.Configurations;

public class DataSeedOptions
{
    public const string SectionName = "DataSeed";
    public const int RetriesCount = 100;

    public int CarsCount { get; set; }
    public int UsersCount { get; set; }
    public int DriversCount { get; set; }
    public int DoctorsCount { get; set; }
    public int MechanicsCount { get; set; }
    public int OperatorsCount { get; set; }
    public int DispatchersCount { get; set; }
}
