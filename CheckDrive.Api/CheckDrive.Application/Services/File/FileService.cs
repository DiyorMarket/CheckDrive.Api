using System.Data;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.QueryParameters;
using Microsoft.AspNetCore.Http;

namespace CheckDrive.Application.Services.File;

public class FileService : IFileService
{
    private readonly IFileCreateService _fileCreateService;
    private readonly IAccountService _accountService;
    private readonly ICarService _carService;

    public FileService(IFileCreateService fileCreateService,
        IAccountService accountService,
        ICarService carService)
    {
        _fileCreateService = fileCreateService;
        _accountService = accountService;
        _carService = carService;
    }

    public async Task<MemoryStream> ExportEmployees(EmployeePosition position, FileQueryParameters queryParameters)
    {

        var dataTable = await GetAccountsDataTable(position);

        var stream = queryParameters.FileType == FileType.Pdf
            ? _fileCreateService.CreatePdf(position.ToString(), dataTable)
            : _fileCreateService.CreateExcel(dataTable);
        return stream;
    }

    public async Task<MemoryStream> ExportCars(FileQueryParameters queryParameters)
    {
        var dataTable = await GetCarsDataTable();

        var stream = queryParameters.FileType == FileType.Pdf
            ? _fileCreateService.CreatePdf("Avtomobillar", dataTable)
            : _fileCreateService.CreateExcel(dataTable);

        return stream;
    }

    public async Task ImportEmployees(IFormFile file)
    {
        var accounts = await FileReadService.ReadExcelDataAsync(file);

        foreach (var account in accounts)
        {
            await _accountService.CreateAsync(account);
        }
    }

    private async Task<DataTable> GetAccountsDataTable(EmployeePosition position)
    {
        var accounts = await _accountService.GetAsync(position);

        var dataTable = new DataTable();

        dataTable.Columns.Add("F.I.SH.", typeof(string));
        dataTable.Columns.Add("Telefon raqami", typeof(string));
        dataTable.Columns.Add("Passport", typeof(string));
        dataTable.Columns.Add("Manizili", typeof(string));
        dataTable.Columns.Add("Kasbi", typeof(string));

        foreach (var account in accounts)
        {
            dataTable.Rows.Add(
                (account.FirstName + " " + account.LastName ?? " "),
                account.PhoneNumber ?? "N/A",
                account.Passport ?? "N/A",
                account.Address ?? "N/A",
                account.Position
            );
        }

        return dataTable;
    }

    private async Task<DataTable> GetCarsDataTable()
    {
        var cars = await _carService.GetAvailableCarsAsync();

        var dataTable = new DataTable();

        dataTable.Columns.Add("Modeli", typeof(string));
        dataTable.Columns.Add("Raqami", typeof(string));
        dataTable.Columns.Add("Rangi", typeof(string));
        dataTable.Columns.Add("Bosib o'tgan masofasi", typeof(string));

        foreach (var car in cars)
        {
            dataTable.Rows.Add(
                car.Model,
                car.Number,
                car.Color ?? " ",
                car.Mileage
            );
        }
        return dataTable;
    }
}
