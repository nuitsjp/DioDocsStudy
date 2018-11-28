using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DioDocsStudy.Pdf;
using GrapeCity.Documents.Pdf;
using GrapeCity.Documents.Pdf.AcroForms;
using GrapeCity.Documents.Text;

namespace SignToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            PdfActivator.Activate();

            using (var inputStream = new FileStream("Invoice.pdf", FileMode.Open))
            using (var outputStream = new FileStream("Signed.pdf", FileMode.Create))
            {
                var doc = new GcPdfDocument();
                doc.Load(inputStream);

                // 署名を保持する署名フィールドを初期化
                var signatureField = new SignatureField();
                signatureField.Widget.Rect = new RectangleF(400, 750, 140, 36);
                signatureField.Widget.Page = doc.Pages.Single();
                signatureField.Widget.BackColor = Color.LightSeaGreen;
                signatureField.Widget.TextFormat.FontName = "游ゴシック";
                // ドキュメントに署名フィールドを追加
                doc.AcroForm.Fields.Add(signatureField);

                // 署名フィールドと署名を結びつけ
                var signatureProperties = new SignatureProperties
                {
                    Certificate = new X509Certificate2(File.ReadAllBytes("diodocs.pfx"), "diodocs"),
                    Location = "My Desktop",
                    SignerName = "DioDocs",
                    SignatureField = signatureField
                };

                // 署名して文書を保存
                // 注意
                // - 署名と保存は一連の操作で、2つは分離できません
                // - Sign()メソッドに渡されたストリームは読み込み可能である必要があります。
                doc.Sign(signatureProperties, outputStream);
            }
        }
    }
}
