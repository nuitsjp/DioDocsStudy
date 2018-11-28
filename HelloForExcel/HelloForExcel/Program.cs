using System;
using DioDocsStudy.Excel;
using GrapeCity.Documents.Excel;

namespace HelloForExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelActivator.Activate();
            // ワークブックの作成
            var workbook = new Workbook();
            // ワークシートの取得
            var worksheet = workbook.ActiveSheet;
            // セル範囲を指定して文字列を設定
            worksheet.Range["C3"].Value = "Hello World!";
            // Excelファイルとして保存
            workbook.Save("Result.xlsm");
        }
    }
}
