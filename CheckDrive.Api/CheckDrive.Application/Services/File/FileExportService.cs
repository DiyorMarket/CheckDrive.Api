using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System.Data;

namespace CheckDrive.Application.Services.File;

public class FileExportService
{
    public MemoryStream CreatePdfWithGrid(string title, DataTable dataTable)
    {
        var document = new PdfDocument();
        var page = document.Pages.Add();

        PdfGraphics graphics = page.Graphics;

        PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
        PdfFont contentFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

        graphics.DrawString(title, titleFont, PdfBrushes.Black, new PointF(10, 10));

        var grid = new PdfGrid();
        grid.DataSource = dataTable;

        var headerStyle = new PdfGridCellStyle
        {
            BackgroundBrush = new PdfSolidBrush(Color.LightGray),
            TextBrush = PdfBrushes.Black,
            Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold),
            StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle),
            Borders = { Left = new PdfPen(Color.Black, 1), Right = new PdfPen(Color.Black, 1), Top = new PdfPen(Color.Black, 1), Bottom = new PdfPen(Color.Black, 1) }
        };

        var cellStyle = new PdfGridCellStyle
        {
            BackgroundBrush = PdfBrushes.White,
            TextBrush = PdfBrushes.Black,
            Font = contentFont,
            StringFormat = new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle),
            Borders = { Left = new PdfPen(Color.Black, 1), Right = new PdfPen(Color.Black, 1), Top = new PdfPen(Color.Black, 1), Bottom = new PdfPen(Color.Black, 1) }
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
    public MemoryStream ExportExcel(DataTable dataTable)
    {
        using var excelEngine = new ExcelEngine();

        IApplication application = excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Excel2016;

        IWorkbook workbook = application.Workbooks.Create(1);
        IWorksheet sheet = workbook.Worksheets[0];

        sheet.ImportDataTable(dataTable, true, 1, 1, true);

        int lastColumn = dataTable.Columns.Count;
        IRange headerRow = sheet.Range[$"A1:{(char)('A' + lastColumn - 1)}1"];
        headerRow.CellStyle.Color = Syncfusion.Drawing.Color.DarkGray;
        headerRow.CellStyle.Font.Color = ExcelKnownColors.Black;

        IListObject table = sheet.ListObjects.Create("Haydovchilar_maulmoti", sheet.UsedRange);
        table.BuiltInTableStyle = TableBuiltInStyles.TableStyleMedium14;

        foreach (IRange row in sheet.UsedRange.Rows)
        {
            if (row.Row != 1)
            {
                row.CellStyle.Color = (row.Row % 2 == 0)
                    ? Syncfusion.Drawing.Color.LightGray
                    : Syncfusion.Drawing.Color.White;
            }
        }

        sheet.UsedRange.AutofitColumns();

        MemoryStream stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }
}
