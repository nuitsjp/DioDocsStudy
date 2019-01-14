using System;
using System.Collections.Generic;
using System.IO;
using GrapeCity.Documents.Excel;

namespace InvoiceBuilder.Report.Impl
{
    public class ReportBuilder<TRowClass>
    {
        private readonly string _tableName;

        private readonly Dictionary<object, Action<IRange>> _setters = new Dictionary<object, Action<IRange>>();

        private readonly Dictionary<object, Action<IRange, TRowClass>> _tableSetter = new Dictionary<object, Action<IRange, TRowClass>>();

        public ReportBuilder(string tableName)
        {
            _tableName = tableName;
        }

        public ReportBuilder<TRowClass> AddSetter(object key, Action<IRange> setter)
        {
            _setters[key] = setter;
            return this;
        }

        public ReportBuilder<TRowClass> AddTableSetter(string key, Action<IRange, TRowClass> setter)
        {
            _tableSetter[key] = setter;
            return this;
        }

        public byte[] Build(Stream stream, IList<TRowClass> rows)
        {
            var workbook = new Workbook();
            workbook.Open(stream);
            var worksheet = workbook.Worksheets[0];

            // 利用している領域を走査して、単一項目を設定する
            var usedRange = worksheet.UsedRange;
            for (var i = 0; i < usedRange.Rows.Count; i++)
            {
                for (var j = 0; j < usedRange.Columns.Count; j++)
                {
                    var cell = usedRange[i, j];
                    if (cell.Value != null && _setters.ContainsKey(cell.Value))
                    {
                        _setters[cell.Value](cell);
                    }
                }
            }

            var templateTable = worksheet.Tables[_tableName];

            // テーブルの行数を確認し、不足分を追加する
            if (templateTable.Rows.Count < rows.Count)
            {
                var addCount = rows.Count - templateTable.Rows.Count;
                for (var i = 0; i < addCount; i++)
                {
                    templateTable.Rows.Add(templateTable.Rows.Count - 1);
                }
            }

            // テーブルの1行目から項目の列番号を探索する
            var rowSetters = new List<(int index, Action<IRange, TRowClass> setter)> ();
            var firstRow = templateTable.Rows[0];
            for (var i = 0; i < firstRow.Range.Columns.Count; i++)
            {
                var value = firstRow.Range[0, i].Value;
                if (value != null && _tableSetter.ContainsKey(value))
                {
                    rowSetters.Add((i, _tableSetter[value]));
                }
            }

            // テーブルに値を設定する
            for (var i = 0; i < templateTable.Rows.Count; i++)
            {
                var row = templateTable.Rows[i];
                foreach (var rowSetter in rowSetters)
                {
                    rowSetter.setter(row.Range[rowSetter.index], rows[i]);
                }
            }

            using (var outputStream = new MemoryStream())
            {
                workbook.Save(outputStream, SaveFileFormat.Pdf);
                return outputStream.ToArray();
            }
        }
    }
}