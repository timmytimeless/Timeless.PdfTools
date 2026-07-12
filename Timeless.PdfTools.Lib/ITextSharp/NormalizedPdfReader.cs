using System;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.ITextSharp;

public class NormalizedPdfReader : PdfReader
{
    public NormalizedPdfReader(byte[] pdf) : base(pdf, null) { }

    public NormalizedPdfRectangle GetNormalizedPageSize(int index)
    {
        return GetNormalizedPageSize(pageRefs.GetPageNRelease(index));
    }

    public NormalizedPdfRectangle GetNormalizedPageSize(PdfDictionary page)    
    {
        var mediaBox = page.GetAsArray(PdfName.Mediabox);
        return CreateNormalizedRectangle(mediaBox);
    }

    public static NormalizedPdfRectangle CreateNormalizedRectangle(PdfArray box)
    {
        var llx = ((PdfNumber)GetPdfObjectRelease(box[0])).FloatValue;
        var lly = ((PdfNumber)GetPdfObjectRelease(box[1])).FloatValue;
        var urx = ((PdfNumber)GetPdfObjectRelease(box[2])).FloatValue;
        var ury = ((PdfNumber)GetPdfObjectRelease(box[3])).FloatValue;

        return new NormalizedPdfRectangle(Math.Min(llx, urx), Math.Min(lly, ury), Math.Max(llx, urx), Math.Max(lly, ury));
    }
}