using System;
using System.Collections.Generic;
using System.IO;
using GrapeCity.Documents.Excel;

namespace ReportService.DioDocs
{
    public class ReportBuilder<TReportRow> : IReportBuilder<TReportRow>, IRange, IDisposable
    {
        private Stream _excel;
        private readonly string _tableName;

        private readonly Dictionary<object, Action<IRange>> _setters = new Dictionary<object, Action<IRange>>();

        private readonly Dictionary<object, Action<IRange, TReportRow>> _tableSetters = new Dictionary<object, Action<IRange, TReportRow>>();

        private GrapeCity.Documents.Excel.IRange _currentRange;

        public object Value
        {
            set => _currentRange.Value = value;
        }

        public ReportBuilder(Stream excel)
        {
            _excel = excel;
            _tableName = typeof(TReportRow).Name;
        }

        public IReportBuilder<TReportRow> AddSetter(object key, Action<IRange> setter)
        {
            _setters[key] = setter;
            return this;
        }

        public IReportBuilder<TReportRow> AddTableSetter(string key, Action<IRange, TReportRow> setter)
        {
            _tableSetters[key] = setter;
            return this;
        }

        public byte[] Build(IList<TReportRow> rows)
        {
            var workbook = new Workbook();
            workbook.Open(_excel);

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
                        _currentRange = cell;
                        _setters[cell.Value](this);
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
            var rowSetters = new List<(int index, Action<IRange, TReportRow> setter)>();
            var firstRow = templateTable.Rows[0];
            for (var i = 0; i < firstRow.Range.Columns.Count; i++)
            {
                var value = firstRow.Range[0, i].Value;
                if (value != null && _tableSetters.ContainsKey(value))
                {
                    rowSetters.Add((i, _tableSetters[value]));
                }
            }

            // テーブルに値を設定する
            for (var i = 0; i < templateTable.Rows.Count; i++)
            {
                var row = templateTable.Rows[i];
                foreach (var rowSetter in rowSetters)
                {
                    _currentRange = row.Range[rowSetter.index];
                    rowSetter.setter(this, rows[i]);
                }
            }

            using (var outputStream = new MemoryStream())
            {
                workbook.Save(outputStream, SaveFileFormat.Pdf);
                return outputStream.ToArray();
            }
        }

        public void Dispose()
        {
            if (_excel != null)
            {
                _excel.Dispose();
                _excel = null;
            }
        }
    }
}