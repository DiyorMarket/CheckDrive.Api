﻿using AutoMapper;
using CheckDricer.Infrastructure.Persistence;
using CheckDrive.Domain.DTOs.Account;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDriver.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly CheckDriveDbContext _context;

        public AccountService(IMapper mapper, CheckDriveDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GetBaseResponse<AccountDto>> GetAccountsAsync(AccountResourceParameters resourceParameters)
        {
            var query = _context.Accounts.AsQueryable();

            if (resourceParameters.RoleId != 0 && resourceParameters.RoleId is not null)
            {
                query = query.Where(x => x.RoleId == resourceParameters.RoleId);
            }
            if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
            {
                query = resourceParameters.OrderBy.ToLowerInvariant() switch
                {
                    "firstname" => query.OrderBy(x => x.FirstName),
                    "firstnamedesc" => query.OrderByDescending(x => x.FirstName),
                    "lastname" => query.OrderBy(x => x.LastName),
                    "lastnamedesc" => query.OrderByDescending(x => x.LastName),
                    "login" => query.OrderBy(x => x.Login),
                    "logindesc" => query.OrderByDescending(x => x.Login),
                    "phonenumber" => query.OrderBy(x => x.PhoneNumber),
                    "phonenumberdesc" => query.OrderByDescending(x => x.PhoneNumber),
                    _ => query.OrderBy(x => x.Id),
                };
            }

            var accounts = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

            var accountDtos = _mapper.Map<List<AccountDto>>(accounts);

            var paginatedResult = new PaginatedList<AccountDto>(accountDtos, accounts.TotalCount, accounts.CurrentPage, accounts.PageSize);

            return paginatedResult.ToResponse();
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            var accountDto = _mapper.Map<AccountDto>(account);

            return accountDto;
        }

        public async Task<AccountDto> CreateAccountAsync(AccountForCreateDto accountForCreate)
        {
            var accountEntity = _mapper.Map<Account>(accountForCreate);

            await _context.Accounts.AddAsync(accountEntity);
            await _context.SaveChangesAsync();

            var accountDto = _mapper.Map<AccountDto>(accountEntity);

            return accountDto;
        }

        public async Task<AccountDto> UpdateAccountAsync(AccountForUpdateDto accountForUpdate)
        {
            var accountEntity = _mapper.Map<Account>(accountForUpdate);

            _context.Accounts.Update(accountEntity);
            await _context.SaveChangesAsync();

            var accountDto = _mapper.Map<AccountDto>(accountEntity);

            return accountDto;
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            if (account is not null)
            {
                _context.Accounts.Remove(account);
            }

            await _context.SaveChangesAsync();
        }
    }
}
