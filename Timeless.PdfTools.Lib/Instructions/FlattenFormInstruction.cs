using System.IO;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class FlattenFormInstruction<TData> : PdfBuildInstruction<TData>
{
    public override byte[] Execute(byte[] pdfBytes, TData data)
    {
        using (var ms = new MemoryStream())
        {
            var pdfReader = new PdfReader(pdfBytes);
            var pdfStamper = new PdfStamper(pdfReader, ms)
            {
                FormFlattening = true
            };

            pdfReader.Close();
            pdfStamper.Close();
            return ms.ToArray();
        }
    }
}