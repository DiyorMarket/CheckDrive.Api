using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Services.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Entities.Identity;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using Moq;
using Moq.EntityFrameworkCore;

namespace CheckDrive.Tests.Unit.Services;

public class DoctorReviewServiceTests : ServiceTestBase
{
    private readonly Mock<ICheckDriveDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly DoctorReviewService _service;

    public DoctorReviewServiceTests()
    {
        _mockContext = new Mock<ICheckDriveDbContext>();
        _mockMapper = new Mock<IMapper>();
        _service = new DoctorReviewService(_mockContext.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidInput_ReturnsCreatedDoctorReviewDto()
    {
        // Arrange
        var createDto = new CreateDoctorReviewDto(
            DriverId: Guid.NewGuid(),
            ReviewerId: Guid.NewGuid(),
            Notes: "Test notes",
            IsApprovedByReviewer: true);

        var doctor = new User { Id = createDto.ReviewerId, Position = EmployeePosition.Doctor };
        var driver = new User { Id = createDto.DriverId, Position = EmployeePosition.Driver };

        var users = new List<User> { doctor, driver }.AsQueryable();
        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        _mockContext.Setup(c => c.DoctorReviews.Add(It.IsAny<DoctorReview>()));

        var expectedDto = new DoctorReviewDto(
            CheckPointId: 1,
            DriverId: createDto.DriverId,
            DriverName: "John Doe",
            ReviewerId: createDto.ReviewerId,
            ReviewerName: "Dr. Smith",
            Notes: createDto.Notes,
            Date: DateTime.UtcNow,
            Status: ReviewStatus.Approved);

        _mockMapper.Setup(m => m.Map<DoctorReviewDto>(It.IsAny<DoctorReview>())).Returns(expectedDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto, result);

        _mockContext.Verify(c => c.DoctorReviews.Add(It.IsAny<DoctorReview>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DoctorNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var createDto = new CreateDoctorReviewDto(
            DriverId: Guid.NewGuid(),
            ReviewerId: Guid.NewGuid(),
            Notes: "Test notes",
            IsApprovedByReviewer: true);

        var users = new List<User>().AsQueryable();
        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_DriverNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var createDto = new CreateDoctorReviewDto(
            DriverId: Guid.NewGuid(),
            ReviewerId: Guid.NewGuid(),
            Notes: "Test notes",
            IsApprovedByReviewer: true);

        var doctor = new User { Id = createDto.ReviewerId, Position = EmployeePosition.Doctor };

        var users = new List<User> { doctor }.AsQueryable();
        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_ReviewerNotDoctor_ThrowsInvalidOperationException()
    {
        // Arrange
        var createDto = new CreateDoctorReviewDto(
            DriverId: Guid.NewGuid(),
            ReviewerId: Guid.NewGuid(),
            Notes: "Test notes",
            IsApprovedByReviewer: true);

        var notDoctor = new User { Id = createDto.ReviewerId, Position = EmployeePosition.Operator };
        var driver = new User { Id = createDto.DriverId, Position = EmployeePosition.Driver };
        var users = new List<User> { notDoctor, driver }.AsQueryable();

        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(createDto));
    }

    [Fact]
    public async Task CreateAsync_NullInput_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null!));
    }
}