﻿using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Repositories;

namespace CheckDrive.Infrastructure.Persistence.Repositories
{
    public class DebtsRepository : RepositoryBase<Debts>, IDebtsRepository
    {
        public DebtsRepository(CheckDriveDbContext dbContext) : base(dbContext) { }
    }
}
