using iTextSharp.text;

namespace Timeless.PdfTools.Lib.Instructions;

public class TestBorderMaker
{
    public static void EnableTestBorder(Image image)
    {
        image.Border = Rectangle.BOTTOM_BORDER + Rectangle.TOP_BORDER + Rectangle.LEFT_BORDER + Rectangle.RIGHT_BORDER;
        image.BorderColor = BaseColor.Red;
        image.BorderWidth = 1;
    }
}