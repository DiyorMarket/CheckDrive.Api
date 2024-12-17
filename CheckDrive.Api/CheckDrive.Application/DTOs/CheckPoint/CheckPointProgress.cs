using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.CheckPoint;

public sealed record CheckPointProgress(int Id, CheckPointStage Stage);
