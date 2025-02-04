﻿using CheckDrive.Application.DTOs.ManagerReview;

namespace CheckDrive.Application.Interfaces.Review;

public interface IManagerReviewService
{
    Task<ManagerReviewDto> GetByIdAsync(int reviewId);
    Task<ManagerReviewDto> CreateAsync(CreateManagerReviewDto review);
}
