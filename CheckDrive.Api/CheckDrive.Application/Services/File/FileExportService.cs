using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Data;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Enums;
using System.IO;
using CheckDrive.Domain.QueryParameters;
using CheckDrive.Application.DTOs.Account;
using Microsoft.AspNetCore.Http;

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

    public async Task<MemoryStream> Export(EmployeePosition position, FileQueryParameters queryParameters)
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
        var accounts = await ReadExcelDataAsync(file);

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

    private async Task<List<CreateAccountDto>> ReadExcelDataAsync(IFormFile file)
    {
        var accountList = new List<CreateAccountDto>();


        using var memoryStream = new MemoryStream();
        
        await file.CopyToAsync(memoryStream);

        memoryStream.Position = 0;

        // Process the Excel file
        using ExcelEngine excelEngine = new ExcelEngine();

        IApplication app = excelEngine.Excel;
        app.DefaultVersion = ExcelVersion.Xlsx;

        // Open workbook from memory stream
        IWorkbook workbook = app.Workbooks.Open(memoryStream, ExcelOpenType.Automatic);
        IWorksheet worksheet = workbook.Worksheets[0];

        // Loop through rows, assuming headers are in row 1
        int rowCount = worksheet.UsedRange.LastRow;
        for (int row = 2; row <= rowCount; row++)
        {
            var username = worksheet[$"A{row}"].Text;
            var fullName = worksheet[$"B{row}"].Text;
            var firstName = fullName.Split(' ')[0];
            var lastName = fullName.Split(' ').Length > 1 ? fullName.Split(' ')[1] : "";

            var account = new CreateAccountDto(
                Username: username, 
                Password: "defaultPassword", 
                PasswordConfirm: "defaultPassword",
                PhoneNumber: worksheet[$"C{row}"].Text,
                Email: worksheet[$"H{row}"].Text,
                FirstName: firstName,
                LastName: lastName,
                Address: worksheet[$"E{row}"].Text,
                Passport: worksheet[$"D{row}"].Text,
                Birthdate: DateTime.TryParse(worksheet[$"F{row}"].Text, out var birthdate) 
                    ? birthdate 
                    : DateTime.MinValue,
                Position: Enum.TryParse<EmployeePosition>(worksheet[$"G{row}"].Text, out var position) 
                    ? position 
                    : throw new InvalidOperationException($"Invalid name of role {worksheet[$"G{row}"]}")
            );
            accountList.Add(account);
        }

        return accountList;
    }
}
