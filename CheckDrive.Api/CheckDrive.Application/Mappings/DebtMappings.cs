using AutoMapper;
using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class DebtMappings : Profile
{
    public DebtMappings()
    {
        CreateMap<Debt, DebtDto>();
    }
}
