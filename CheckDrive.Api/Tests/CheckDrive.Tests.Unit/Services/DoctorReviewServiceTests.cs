using AutoFixture;
using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.Hubs;
using CheckDrive.Application.Services.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Moq.EntityFrameworkCore;

namespace CheckDrive.Tests.Unit.Services;

public class DoctorReviewServiceTests : ServiceTestBase
{
    private readonly Mock<ICheckDriveDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DoctorReviewService _service;
    private readonly Mock<IHubContext<ReviewHub, IReviewHub>> _mockHubContext;

    public DoctorReviewServiceTests()
    {
        _mockContext = new Mock<ICheckDriveDbContext>();
        _mockMapper = new Mock<IMapper>();
        _mockHubContext = new Mock<IHubContext<ReviewHub, IReviewHub>>();
        _service = new DoctorReviewService(_mockContext.Object, _mockMapper.Object, _mockHubContext.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidInput_ReturnsCreatedDoctorReviewDto()
    {
        // Arrange
        var doctor = _fixture.Create<Doctor>();
        var driver = _fixture.Create<Driver>();
        var createDto = _fixture.Build<CreateDoctorReviewDto>()
            .With(d => d.DriverId, driver.Id)
            .With(d => d.DoctorId, doctor.Id)
            .Create();

        _mockContext.Setup(c => c.Doctors).ReturnsDbSet(new List<Doctor> { doctor });
        _mockContext.Setup(c => c.Drivers).ReturnsDbSet(new List<Driver> { driver });
        _mockContext.Setup(c => c.DoctorReviews.Add(It.IsAny<DoctorReview>()));

        var expectedDto = _fixture.Create<DoctorReviewDto>();
        _mockMapper.Setup(m => m.Map<DoctorReviewDto>(It.IsAny<DoctorReview>())).Returns(expectedDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DoctorNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var createDto = _fixture.Create<CreateDoctorReviewDto>();
        _mockContext.Setup(c => c.Doctors).ReturnsDbSet(new List<Doctor>());

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_DriverNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var doctor = _fixture.Create<Doctor>();
        var createDto = _fixture.Build<CreateDoctorReviewDto>()
            .With(d => d.DoctorId, doctor.Id)
            .Create();

        _mockContext.Setup(c => c.Doctors).ReturnsDbSet(new List<Doctor> { doctor });
        _mockContext.Setup(c => c.Drivers).ReturnsDbSet(new List<Driver>());

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_NullInput_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null!));
    }

    [Fact]
    public async Task CreateAsync_RejectedReview_CreatesCheckPointWithCorrectStatus()
    {
        // Arrange
        var doctor = _fixture.Create<Doctor>();
        var driver = _fixture.Create<Driver>();
        var createDto = _fixture.Build<CreateDoctorReviewDto>()
            .With(d => d.DriverId, driver.Id)
            .With(d => d.DoctorId, doctor.Id)
            .Create();

        _mockContext.Setup(c => c.Doctors).ReturnsDbSet(new List<Doctor> { doctor });
        _mockContext.Setup(c => c.Drivers).ReturnsDbSet(new List<Driver> { driver });

        DoctorReview capturedReview = null;
        _mockContext.Setup(c => c.DoctorReviews.Add(It.IsAny<DoctorReview>()))
            .Callback<DoctorReview>(r => capturedReview = r);

        // Act
        await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(capturedReview);
        Assert.Equal(CheckPointStatus.Completed, capturedReview.CheckPoint.Status);
    }
}