using ClosedXML.Excel;
using System;
using System.Collections.Generic;

namespace UI
{
    public class ReportGenerator
    {
        private readonly List<ReportEntry> _reportEntries;

        public ReportGenerator()
        {
            _reportEntries = new List<ReportEntry>();
        }

        // Додавання запису до звіту
        public void AddEntry(string searchOrSortType, string criteria,
            int resultsCount, string sortOrder = null)
        {
            _reportEntries.Add(new ReportEntry
            {
                OrderNumber = _reportEntries.Count + 1,
                SearchOrSortType = searchOrSortType,
                Criteria = criteria,
                ResultsCount = resultsCount,
                SortOrder = sortOrder,
                Timestamp = DateTime.Now
            });
        }

        // Збереження звіту у файл XLSX
        public void GenerateReport(string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Search Report");

                // Створення заголовків таблиці
                worksheet.Cell(1, 1).Value = "№";
                worksheet.Cell(1, 2).Value = "Тип пошуку/сортування";
                worksheet.Cell(1, 3).Value = "Критерій пошуку/сортування";
                worksheet.Cell(1, 4).Value = "Напрямок сортування";
                worksheet.Cell(1, 5).Value = "Кількість записів";
                worksheet.Cell(1, 6).Value = "Час виконання";

                // Додавання стилів для заголовків
                var headerRange = worksheet.Range(1, 1, 1, 6);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Додавання даних
                int currentRow = 2;
                foreach (var entry in _reportEntries)
                {
                    worksheet.Cell(currentRow, 1).Value = entry.OrderNumber;
                    worksheet.Cell(currentRow, 2).Value = entry.SearchOrSortType;
                    worksheet.Cell(currentRow, 3).Value = entry.Criteria;
                    worksheet.Cell(currentRow, 4).Value = entry.SortOrder ?? "N/A";
                    worksheet.Cell(currentRow, 5).Value = entry.ResultsCount;
                    worksheet.Cell(currentRow, 6).Value = entry.Timestamp;

                    currentRow++;
                }

                // Автоматичне підлаштування ширини колонок
                worksheet.Columns().AdjustToContents();

                // Додавання опису
                worksheet.Cell(currentRow + 1, 1).Value = "Звіт про пошуки та сортування фільмів.";
                worksheet.Cell(currentRow + 2, 1).Value = "Кожен рядок представляє одну операцію з відповідними даними.";
                worksheet.Range(currentRow + 1, 1, currentRow + 2, 6).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Збереження в файл
                workbook.SaveAs(filePath);
            }
        }
    }

    // Модель даних для запису в звіт
    public class ReportEntry
    {
        public int OrderNumber { get; set; }
        public string SearchOrSortType { get; set; }
        public string Criteria { get; set; }
        public int ResultsCount { get; set; }
        public string SortOrder { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
