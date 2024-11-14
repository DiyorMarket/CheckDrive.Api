using CheckDrive.Application.DTOs.Account;
using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Syncfusion.XlsIO;

namespace CheckDrive.Application.Services.File;

public class FileReadService
{
    public static async Task<List<CreateAccountDto>> ReadExcelDataAsync(IFormFile file)
    {
        var accountList = new List<CreateAccountDto>();

        using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream);

        memoryStream.Position = 0;

        using ExcelEngine excelEngine = new ExcelEngine();

        IApplication app = excelEngine.Excel;
        app.DefaultVersion = ExcelVersion.Xlsx;

        IWorkbook workbook = app.Workbooks.Open(memoryStream, ExcelOpenType.Automatic);
        IWorksheet worksheet = workbook.Worksheets[0];

        int rowCount = worksheet.UsedRange.LastRow;
        for (int row = 2; row <= rowCount; row++)
        {
            var firstName = worksheet[$"A{row}"].Text;
            var lastName = worksheet[$"B{row}"].Text;
            var username = worksheet[$"C{row}"].Text;
            var password = worksheet[$"D{row}"].Text;
            var phoneNumber = worksheet[$"E{row}"].Text;
            var address = worksheet[$"F{row}"].Text;
            var passport = worksheet[$"G{row}"].Text;
            var birthdateText = worksheet[$"H{row}"].Text;
            var positionText = worksheet[$"I{row}"].Text;
            var email = worksheet[$"J{row}"].Text;

            DateTime birthdate = DateTime.TryParse(birthdateText, out var parsedBirthdate)
                ? parsedBirthdate
                : DateTime.MinValue;

            EmployeePosition position = Enum.TryParse<EmployeePosition>(positionText, out var parsedPosition)
                ? parsedPosition
                : throw new InvalidOperationException($"Invalid name of role {positionText}");

            var account = new CreateAccountDto(
                Username: username,
                Password: password,
                PasswordConfirm: password,
                PhoneNumber: phoneNumber,
                Email: email,
                FirstName: firstName,
                LastName: lastName,
                Address: address,
                Passport: passport,
                Birthdate: birthdate,
                Position: position
            );

            accountList.Add(account);
        }

        return accountList;
    }
}
