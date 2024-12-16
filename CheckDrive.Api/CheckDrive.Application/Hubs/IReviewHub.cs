namespace CheckDrive.Application.Hubs;

public interface IReviewHub
{
    Task CheckPointProgressUpdated(int checkPointId);
}
