namespace CheckDrive.Tests.Api.Helpers;

public static class ApiUrlHelper
{
    public const string DoctorReview = "reviews/doctors/";

    public static string GetDoctorReviewUrl(int doctorId)
        => $"reviews/doctors/{doctorId}";
}
