using System.IO;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class RotatePdfInstruction<TData> : PdfBuildInstruction<TData>
{
    public override byte[] Execute(byte[] pdfBytes, TData data)
    {
        using (var ms = new MemoryStream())
        {
            var pdfReader = new PdfReader(pdfBytes);

            var pagesCount = pdfReader.NumberOfPages;

            for (var n = 1; n <= pagesCount; n++)
            {
                var page = pdfReader.GetPageN(n);
                var rotate = page.GetAsNumber(PdfName.Rotate);
                var rotation = rotate == null ?
                    90 :
                    (rotate.IntValue + 90) % 360;

                page.Put(PdfName.Rotate, new PdfNumber(rotation));
            }

            var pdfStamper = new PdfStamper(pdfReader, ms)
            {
                //RotateContents = true,
            };

            pdfReader.Close();
            pdfStamper.Close();
            return ms.ToArray();
        }
    }
}