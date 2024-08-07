﻿using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class OperatorReviewRepository : RepositoryBase<OperatorReview>, IOperatorReviewRepository
    {
        public OperatorReviewRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
