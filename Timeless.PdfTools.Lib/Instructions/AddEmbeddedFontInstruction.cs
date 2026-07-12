using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class AddEmbeddedFontInstruction<TData> : PdfBuildInstruction<TData>
{
    private readonly BaseFont _baseFont;

    public AddEmbeddedFontInstruction(byte[] fontBytes, string fontName)
    {
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
            var form = pdfStamper.AcroFields;

            foreach (var key in form.Fields.Keys.Cast<string>())
            {
                form.SetFieldProperty(key, "textfont", _baseFont, null);
                form.RegenerateField(key);
            }
                
            pdfReader.Close();
            pdfStamper.Close();
            return ms.ToArray();
        }
    }
}