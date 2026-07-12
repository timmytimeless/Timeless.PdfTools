using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class AddEmbeddedFontForFieldInstruction<TData> : PdfBuildInstruction<TData>
{
    private readonly string _fieldName;
    private readonly BaseFont _baseFont;

    public AddEmbeddedFontForFieldInstruction(string fieldName, byte[] fontBytes, string fontName)
    {
        _fieldName = fieldName;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _baseFont = BaseFont.CreateFont($"{fontName}.ttf", PdfObject.TEXT_PDFDOCENCODING, BaseFont.EMBEDDED, true, fontBytes, null);
        _baseFont.Subset = false;
    }

    public override byte[] Execute(byte[] pdfBytes, TData data)
    {
        using (var ms = new MemoryStream())
        {
            var pdfReader = new PdfReader(pdfBytes);
            var pdfStamper = new PdfStamper(pdfReader, ms);
            {
                var form = pdfStamper.AcroFields;
                var field = form.Fields.Keys.Cast<string>().SingleOrDefault(x => x == _fieldName);
                form.SetFieldProperty(field, "textfont", _baseFont, null);
                form.RegenerateField(field);
            }

            pdfReader.Close();
            pdfStamper.Close();
            return ms.ToArray();
        }
    }
}