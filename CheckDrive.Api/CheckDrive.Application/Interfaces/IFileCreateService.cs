using System.Data;

namespace CheckDrive.Application.Interfaces;

public interface IFileCreateService
{
    MemoryStream CreatePdf(DataTable dataTable);
    MemoryStream CreateExcel(DataTable dataTable);
}
