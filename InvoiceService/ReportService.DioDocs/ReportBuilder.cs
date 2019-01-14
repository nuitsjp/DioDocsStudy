using System;
using System.Collections.Generic;
using System.IO;
using GrapeCity.Documents.Excel;

namespace ReportService.DioDocs
{
    public class ReportBuilder<TReport, TReportRow> : IReportBuilder<TReport, TReportRow>, IField
    {
        private readonly string _tableName;

        private readonly ITemplateProvider _templateProvider;

        private readonly Dictionary<object, Action<IField>> _setters = new Dictionary<object, Action<IField>>();

        private readonly Dictionary<object, Action<IField, TReportRow>> _tableSetters = new Dictionary<object, Action<IField, TReportRow>>();

        private IRange _currentRange;

        public object Value
        {
            set => _currentRange.Value = value;
        }

        public ReportBuilder(string tableName, ITemplateProvider templateProvider)
        {
            _tableName = tableName;
            _templateProvider = templateProvider;
        }

        public IReportBuilder<TReport, TReportRow> AddSetter(object key, Action<IField> setter)
        {
            _setters[key] = setter;
            return this;
        }

        public IReportBuilder<TReport, TReportRow> AddTableSetter(string key, Action<IField, TReportRow> setter)
        {
            _tableSetters[key] = setter;
            return this;
        }

        public byte[] Build(IList<TReportRow> rows)
        {
            using (var stream = new MemoryStream(_templateProvider.Get<TReport>()))
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
                var rowSetters = new List<(int index, Action<IField, TReportRow> setter)>();
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
        }
    }
}