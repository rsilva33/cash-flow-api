﻿using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "€";

    private readonly IExpensesReadOnlyRepository _repository;

    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository) =>
        _repository = repository;

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _repository.FilterByMonth(month);

        if (expenses.Count is 0)
            return [];

        var workBook = Informations();

        var workSheet = workBook.Worksheets.Add(month.ToString("Y"));

        InsertHeader(workSheet);

        InsertValues(expenses, workSheet);

        workSheet.Columns().AdjustToContents();

        var file = new MemoryStream();

        workBook.SaveAs(file);

        return file.ToArray();
    }

    private static XLWorkbook Informations()
    {
        using var workBook = new XLWorkbook();

        workBook.Author = "Ronaldo Silva";
        workBook.Style.Font.FontSize = 12;
        workBook.Style.Font.FontName = "Times New Roman";

        return workBook;
    }

    private static void InsertHeader(IXLWorksheet workSheet)
    {
        workSheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        workSheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        workSheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        workSheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        workSheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        workSheet.Cells("A1:E1").Style.Font.Bold = true;

        workSheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

        workSheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        workSheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        workSheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        workSheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        workSheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }

    private static void InsertValues(List<Expense> expenses, IXLWorksheet workSheet)
    {
        var raw = 2;

        foreach (var expense in expenses)
        {
            workSheet.Cell($"A{raw}").Value = expense.Title;
            workSheet.Cell($"B{raw}").Value = expense.Date;
            workSheet.Cell($"C{raw}").Value = expense.PaymentType.PaymentTypeToString();

            workSheet.Cell($"D{raw}").Value = expense.Amount;
            workSheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";

            workSheet.Cell($"E{raw}").Value = expense.Description;

            raw++;
        }
    }
}
