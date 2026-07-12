using iTextSharp.text;

namespace Timeless.PdfTools.Lib.ITextSharp;

public class NormalizedPdfRectangle : Rectangle
{
    public NormalizedPdfRectangle(float llx, float lly, float urx, float ury) : base(llx, lly, urx, ury) { }

    public NormalizedPdfRectangle(RectangleJ rectangle) : this(rectangle.X, rectangle.Y, rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height) { }
}