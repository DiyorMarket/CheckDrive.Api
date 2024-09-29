using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Tests.Api.Helpers;
using System.Net;
using Xunit.Abstractions;

namespace CheckDrive.Tests.Api.Endpoints.Reviews.DoctorReview;

public class CreateDoctorReviewTests : EndpointTestBase
{
    public CreateDoctorReviewTests(TestingWebApplicationFactory factory, ITestOutputHelper outputHelper)
        : base(factory, outputHelper)
    {
    }

    [Fact(Skip = "Temporary CI Issues")]
    public async Task CreateDoctorReview_ShouldCreateReview_WhenPassedValidRequest()
    {
        // Arrange
        var driverId = await GetRandomIdAsync<Driver>();
        var doctorId = await GetRandomIdAsync<Doctor>();
        var request = new CreateDoctorReviewDto(
            DriverId: driverId,
            ReviewerId: doctorId,
            Notes: _faker.Lorem.Sentence(),
            IsApprovedByReviewer: _faker.Random.Bool());

        // Act
        var response = await _client.PostAsync<DoctorReviewDto>(ApiUrlHelper.GetDoctorReviewUrl(doctorId), request, HttpStatusCode.Created);

        // Assert
        await _responseValidator.DoctorReview.ValidateCreateAsync(request, response);
    }
}
