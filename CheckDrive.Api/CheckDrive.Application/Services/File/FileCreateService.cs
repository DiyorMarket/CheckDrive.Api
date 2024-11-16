using CheckDrive.Application.Interfaces;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using System.Data;
using Syncfusion.Drawing;

namespace CheckDrive.Application.Services.File;

internal class FileCreateService : IFileCreateService
{
    public MemoryStream CreateExcel(DataTable dataTable)
    {
        using var excelEngine = new ExcelEngine();

        var application = excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Excel2016;

        var workbook = application.Workbooks.Create(1);
        var sheet = workbook.Worksheets[0];

        sheet.ImportDataTable(dataTable, true, 1, 1, true);

        int lastColumn = dataTable.Columns.Count;
        var headerRow = sheet.Range[$"A1:{(char)('A' + lastColumn - 1)}1"];
        headerRow.CellStyle.Color = Syncfusion.Drawing.Color.DarkGray;
        headerRow.CellStyle.Font.Color = ExcelKnownColors.Black;

        var table = sheet.ListObjects.Create("Haydovchilar_maulmoti", sheet.UsedRange);
        table.BuiltInTableStyle = TableBuiltInStyles.TableStyleMedium14;

        foreach (var row in sheet.UsedRange.Rows)
        {
            if (row.Row != 1)
            {
                row.CellStyle.Color = (row.Row % 2 == 0)
                    ? Syncfusion.Drawing.Color.LightGray
                    : Syncfusion.Drawing.Color.White;
            }
        }

        sheet.UsedRange.AutofitColumns();

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    public MemoryStream CreatePdf(string title, DataTable dataTable)
    {
        var document = new PdfDocument();
        var page = document.Pages.Add();

        var graphics = page.Graphics;

        var titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
        var contentFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

        graphics.DrawString(title, titleFont, PdfBrushes.Black, new PointF(10, 10));

        var grid = new PdfGrid();
        grid.DataSource = dataTable;

        var headerStyle = new PdfGridCellStyle
        {
            BackgroundBrush = new PdfSolidBrush(Color.LightGray),
            TextBrush = PdfBrushes.Black,
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold),
            StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle),
            Borders =
            {
                Left = new PdfPen(Color.Black, 1),
                Right = new PdfPen(Color.Black, 1),
                Top = new PdfPen(Color.Black, 1),
                Bottom = new PdfPen(Color.Black, 1)
            }
        };

        var cellStyle = new PdfGridCellStyle
        {
            BackgroundBrush = PdfBrushes.White,
            TextBrush = PdfBrushes.Black,
            Font = contentFont,
            StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle),
            Borders =
            {
                Left = new PdfPen(Color.Black, 1),
                Right = new PdfPen(Color.Black, 1),
                Top = new PdfPen(Color.Black, 1),
                Bottom = new PdfPen(Color.Black, 1)
            }
        };

        foreach (PdfGridCell cell in grid.Headers[0].Cells)
        {
            cell.Style = headerStyle;
        }

        foreach (PdfGridRow row in grid.Rows)
        {
            foreach (PdfGridCell cell in row.Cells)
            {
                cell.Style = cellStyle;
            }
        }

        grid.Draw(page, new PointF(10, 40));

        var stream = new MemoryStream();
        document.Save(stream);
        stream.Position = 0;

        return stream;
    }
}
