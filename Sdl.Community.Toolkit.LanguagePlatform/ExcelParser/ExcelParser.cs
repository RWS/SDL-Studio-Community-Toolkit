using ExcelDataReader;
using OfficeOpenXml;
using Sdl.Community.Toolkit.LanguagePlatform.Models;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sdl.Community.Toolkit.LanguagePlatform.ExcelParser
{
    public class ExcelParser
    {
        public List<ExcelSheet> ReadExcel(string excelPath, int sheetNumber)
        {
            var excelFileResults = new List<ExcelSheet>();
           
            using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
            {
                using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    var result = excelReader.AsDataSet();
                    var dataTable = result.Tables[sheetNumber];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var excelSheet = new ExcelSheet
                        {
                            RowValues = row.ItemArray
                        };
                        excelFileResults.Add(excelSheet);
                    }
                }
            }
            return excelFileResults;
        }

        /// <summary>
        /// Update the Excel local file using the user's input.
        /// </summary>
        /// <param name="mtCodeExcel">mtCodeExcel model</param>
        public async Task UpdateMTCodeExcel(MTCodeExcel mtCodeExcel)
        {
            var file = new FileInfo(mtCodeExcel.ExcelPath);
            using (var excelPackage = new ExcelPackage(file))
            {
                var excelWorkBook = excelPackage.Workbook;
                var excelWorksheet = excelWorkBook.Worksheets.FirstOrDefault(w => w.Name.Equals(mtCodeExcel.ExcelSheet));
                excelWorksheet.Cells[mtCodeExcel.SheetRowNumber, mtCodeExcel.LocaleColumnNumber].Value = mtCodeExcel.LocaleValue;
                excelWorksheet.Cells[mtCodeExcel.SheetRowNumber, mtCodeExcel.MainColumnNumber].Value = mtCodeExcel.MainValue;
                excelWorksheet.Cells[mtCodeExcel.SheetRowNumber, mtCodeExcel.LanguageColumnNumber].Value = mtCodeExcel.Language;
                excelWorksheet.Cells[mtCodeExcel.SheetRowNumber, mtCodeExcel.RegionColumnNumber].Value = mtCodeExcel.Region;
                excelWorksheet.Cells[mtCodeExcel.SheetRowNumber, mtCodeExcel.TradosCodeColumnNumber].Value = mtCodeExcel.TradosCode;
                excelPackage.Save();
            }
        }
    }
}