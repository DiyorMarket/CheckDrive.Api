using System.Data;

namespace CheckDrive.Application.Interfaces;

public interface IFileCreateService
{
    MemoryStream CreatePdf(string name, DataTable dataTable);
    MemoryStream CreateExcel(DataTable dataTable);
}
