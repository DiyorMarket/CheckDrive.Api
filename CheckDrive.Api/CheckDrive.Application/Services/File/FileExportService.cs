using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Data;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Enums;
using System.IO;

namespace CheckDrive.Application.Services.File;

public class FileExportService : IFileExportService
{
    private readonly IFileCreateService _fileCreateService;
    private readonly IAccountService _accountService;
    private readonly ICarService _carService;

    public FileExportService(IFileCreateService fileCreateService,
        IAccountService accountService, 
        ICarService carService)
    {
        _fileCreateService = fileCreateService;
        _accountService = accountService;
        _carService = carService;
    }

    public async Task<MemoryStream> ExportPdf(EmployeePosition position)
    {
        var dataTable = await GetAccountsDataTable(position);
        var stream = _fileCreateService.CreatePdf(position.ToString(), dataTable);

        return stream;
    }

    public async Task<MemoryStream> ExportCarsPdf()
    {
        var dataTable = await GetCarsDataTable();
        var stream = _fileCreateService.CreatePdf("Avtomobillar", dataTable);

        return stream;
    }

    public async Task<MemoryStream> ExportExcel(EmployeePosition position)
    {
        var dataTable = await GetAccountsDataTable(position);
        var stream = _fileCreateService.CreateExcel(dataTable);

        return stream;
    }

    public async Task<MemoryStream> ExportCarsExcel()
    {
        var dataTable = await GetCarsDataTable();
        var stream = _fileCreateService.CreateExcel(dataTable);

        return stream;
    }

    private async Task<DataTable> GetAccountsDataTable(EmployeePosition position)
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
