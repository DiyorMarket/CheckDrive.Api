using CheckDrive.Application.DTOs.Reports;
using CheckDrive.Application.Interfaces.Reports;
using Syncfusion.XlsIO;

namespace CheckDrive.Infrastructure.Reports;

internal sealed class ReportGenerator : IReportGenerator
{
    public MemoryStream Generate(MonthlyReportSummary summary)
    {
        using var excelEngine = new ExcelEngine();
        IApplication application = excelEngine.Excel;
        application.DefaultVersion = ExcelVersion.Xlsx;
        IWorkbook workbook = application.Workbooks.Create(2);

        IWorksheet carWorksheet = workbook.Worksheets[0];
        carWorksheet.Name = "Avtomobillar oylik xisoboti";
        FormatCarSheet(carWorksheet, summary);

        IWorksheet oilWorksheet = workbook.Worksheets[1];
        oilWorksheet.Name = "Yoqilg'ilar oylik xisoboti";
        FormatOilMarkSheet(oilWorksheet, summary.OilMarkConsumptionSummaries);


        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return stream;
    }

    private static void FormatCarSheet(IWorksheet workSheet, MonthlyReportSummary summary)
    {
        // Title
        workSheet.Range["A2:D2"].Merge();
        workSheet.Range["A2"].Text = "Oylik avtomobillar xisoboti";
        workSheet.Range["A2"].CellStyle.Font.Bold = true;
        workSheet.Range["A2"].CellStyle.Font.Size = 20;
        workSheet.Range["A2"].CellStyle.Font.FontName = "Verdana";
        workSheet.Range["A2"].CellStyle.Font.RGBColor = Syncfusion.Drawing.Color.FromArgb(0, 0, 112, 192);
        workSheet.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

        // Summary
        workSheet.Range["A4"].Text = "Yaratilgan sana";
        workSheet.Range["B4"].NumberFormat = "mm/dd/yyyy hh:mm";
        workSheet.Range["B4"].DateTime = DateTime.Now;
        workSheet.Range["A5"].Text = "Ta'mirdagi avtomobillar soni";
        workSheet.Range["B5"].Number = (double)summary.OutOfServiceCarsCount;
        workSheet.Range["A6"].Text = "Limitga yetgan avtomobillar soni";
        workSheet.Range["B6"].Number = (double)summary.LimitsExceededCarsCount;
        workSheet.Range["A7"].Text = "Limitga yaqinlashgan avtomobillar soni";
        workSheet.Range["B7"].Number = summary.TotalAnomalies;
        workSheet.Range["A4:A7"].CellStyle.Font.Bold = true;
        workSheet.Range["B4:B7"].CellStyle.Font.Bold = true;
        workSheet.Range["A4:A7"].CellStyle.Font.RGBColor = Syncfusion.Drawing.Color.FromArgb(0, 128, 128, 128);
        workSheet.Range["B4:B7"].CellStyle.Font.RGBColor = Syncfusion.Drawing.Color.FromArgb(0, 174, 170, 170);
        workSheet.Range["A4:B7"].CellStyle.Font.FontName = "Verdana";

        // Main Report Table Headers
        workSheet.Range["A9:I9"].CellStyle.Color = Syncfusion.Drawing.Color.FromArgb(0, 0, 112, 192);
        workSheet.Range["A9:I9"].CellStyle.Font.Bold = true;
        workSheet.Range["A9:I9"].CellStyle.Font.Color = ExcelKnownColors.White;
        workSheet.Range["A9:I9"].CellStyle.Font.FontName = "Verdana";
        workSheet.Range["A9:I9"].CellStyle.Font.Size = 10;
        workSheet.Range["A9"].Text = "Avtomobil";
        workSheet.Range["B9"].Text = "Yoqilg'i turi";
        workSheet.Range["C9"].Text = "O'rtacha yoqilg'i sarfi";
        workSheet.Range["D9"].Text = "Bosib o'tilgan masofa";
        workSheet.Range["E9"].Text = "Kutilgan yoqilg'i sarfi";
        workSheet.Range["F9"].Text = "Asl yoqilg'i sarfi";
        workSheet.Range["G9"].Text = "Quyilgan yoqilg'i hajmi";
        workSheet.Range["H9"].Text = "Boshlang'ich yoqilg'i hajmi";
        workSheet.Range["I9"].Text = "Yakuniy yoqilg'i hajmi";

        int row = 10;
        foreach (var car in summary.CarUsageSummaries)
        {
            workSheet.Range[$"A{row}:I{row}"].CellStyle.Font.FontName = "Verdana";
            workSheet.Range[$"A{row}:I{row}"].CellStyle.Font.Size = 10;

            if (row % 2 == 0)
            {
                workSheet.Range[$"A{row}:I{row}"].CellStyle.Color = Syncfusion.Drawing.Color.LightGray;
            }

            workSheet.Range[$"A{row}"].Text = car.CarName;
            workSheet.Range[$"B{row}"].Text = "Ai-" + car.OilMark;
            workSheet.Range[$"C{row}"].Number = (double)car.AverageFuelConsumptionLimit;
            workSheet.Range[$"D{row}"].Number = car.TotalDistanceTraveled;
            workSheet.Range[$"E{row}"].Number = (double)car.ExpectedFuelConsumption;
            workSheet.Range[$"F{row}"].Number = (double)car.ActualFuelConsumed;
            workSheet.Range[$"G{row}"].Number = (double)car.TotalFuelAdded;
            workSheet.Range[$"H{row}"].Number = (double)car.StartingFuelLeftover;
            workSheet.Range[$"I{row}"].Number = (double)car.EndingFuelLeftover;
            row++;
        }

        workSheet.UsedRange.AutofitColumns();
    }

    private static void FormatOilMarkSheet(IWorksheet sheet, List<OilMarkConsumptionSummary> oilMarkSummary)
    {
        sheet.Range["A2:C2"].Merge();
        sheet.Range["A2"].Text = "Oylik yoqilg'ilar xisoboti";
        sheet.Range["A2"].CellStyle.Font.Bold = true;
        sheet.Range["A2"].CellStyle.Font.Size = 20;
        sheet.Range["A2"].CellStyle.Font.FontName = "Verdana";
        sheet.Range["A2"].HorizontalAlignment = ExcelHAlign.HAlignCenter;
        sheet.Range["A2"].CellStyle.Font.RGBColor = Syncfusion.Drawing.Color.FromArgb(0, 0, 112, 192);

        sheet.Range["A4:C4"].CellStyle.Color = Syncfusion.Drawing.Color.FromArgb(0, 0, 112, 192);
        sheet.Range["A4:C4"].CellStyle.Font.Bold = true;
        sheet.Range["A4:C4"].CellStyle.Font.Color = ExcelKnownColors.White;
        sheet.Range["A4:C4"].CellStyle.Font.FontName = "Verdana";
        sheet.Range["A4:C4"].CellStyle.Font.Size = 10;
        sheet.Range["A4"].Text = "Yoqilg'i markasi";
        sheet.Range["B4"].Text = "Bu oy yoqilg'i sarfi";
        sheet.Range["C4"].Text = "O'tgan oy yoqilg'i sarfi";

        int row = 5;
        foreach (var oil in oilMarkSummary)
        {
            sheet.Range[$"A{row}:C{row}"].CellStyle.Font.FontName = "Verdana";
            sheet.Range[$"A{row}:C{row}"].CellStyle.Font.Size = 10;
            if (row % 2 == 0)
            {
                sheet.Range[$"A{row}:C{row}"].CellStyle.Color = Syncfusion.Drawing.Color.LightGray;
            }
            sheet.Range[$"A{row}"].Text = "Ai-" + oil.OilMark;
            sheet.Range[$"B{row}"].Number = (double)oil.TotalFuelConsumed;
            sheet.Range[$"C{row}"].Number = (double)oil.PreviousMonthConsumption;
            row++;
        }
        sheet.UsedRange.AutofitColumns();
    }
}
