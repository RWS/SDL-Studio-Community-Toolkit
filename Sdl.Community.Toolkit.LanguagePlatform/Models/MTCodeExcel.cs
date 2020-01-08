namespace Sdl.Community.Toolkit.LanguagePlatform.Models
{
    public class MTCodeExcel
    {
        /// <summary>
        /// Gets or sets the Excel path
        /// </summary>
        public string ExcelPath { get; set; }

        /// <summary>
        /// Gets or sets the Excel Sheet name: E.g: Sheet1
        /// </summary>
        public string ExcelSheet { get; set; }

        /// <summary>
        /// Gets or sets the MTCodeLocale value which will be set/updated in Excel file
        /// </summary>
        public string LocaleValue { get; set; }

        /// <summary>
        /// Gets or sets the row number which should be used to update cell in Excel file
        /// </summary>
        public int SheetRowNumber { get; set; }

        /// <summary>
        /// Gets or sets the MTCodeLocale column number from Excel sheet (e.g: 5)
        /// </summary>
        public int LocaleColumnNumber { get; set; }

        /// <summary>
        /// Gets or sets the MTCodeMain value which will be set/updated in Excel file
        /// </summary>
        public string MainValue { get; set; } 

        /// <summary>
        /// Gets or sets the MTCodeMain column number from Excel sheet (e.g: 4)
        /// </summary>
        public int MainColumnNumber { get; set; }

        /// <summary>
        /// Gets or sets the Language value from Excel sheet
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the Language column number from Excel sheet (e.g: 1)
        /// </summary>
        public int LanguageColumnNumber { get; set; }

        /// <summary>
        /// Gets or sets the Region value from Excel sheet
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the Region column number from Excel sheet (e.g: 2)
        /// </summary>
        public int RegionColumnNumber { get; set; }

        /// <summary>
        /// Gets or sets the TradosCode value from Excel sheet
        /// </summary>
        public string TradosCode { get; set; }

        /// <summary>
        /// Gets or sets the TradosCode column number from Excel sheet (e.g: 3)
        /// </summary>
        public int TradosCodeColumnNumber { get; set; }
    }
}