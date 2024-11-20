namespace CheckDrive.TestDataCreator.Interfaces;

public interface IDatabaseSeederFactory
{
    IDatabaseSeeder CreateSeeder(string environment);
}
