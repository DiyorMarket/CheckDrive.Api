using CheckDrive.Application.DTOs.Account;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Enums;
using System.Data;

namespace CheckDrive.Application.Services.File;
internal class AccountFileExports
{
    private readonly IAccountService _accountService;
    private readonly FileExportService _fileExportService = new FileExportService();

    public AccountFileExports(IAccountService accountService)
    {
        _accountService = accountService;
    }

    private async Task<DataTable> GetAsync(EmployeePosition position)
    {
        var accounts = await _accountService.GetAsync(position);
        
        var dataTable = new DataTable();

        dataTable.Columns.Add("F.I.SH.", typeof(string));
        dataTable.Columns.Add("Telefon raqami", typeof(string));
        dataTable.Columns.Add("Passport", typeof(string));
        dataTable.Columns.Add("Manizili", typeof(string));

        foreach (var account in accounts)
        {
            dataTable.Rows.Add(
                (account.FirstName + " " + account.LastName ?? " "),
                account.PhoneNumber ?? "N/A",
                account.Passport ?? "N/A",
                account.Address ?? "N/A"
            );
        }

        return dataTable;
    }
}
