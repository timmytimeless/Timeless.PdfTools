using iTextSharp.text;

namespace Timeless.PdfTools.Lib.ITextSharp;

public static class MeasurementExtensions
{
    public static float MillimetersToPoints(this float millimeters)
    {
        return Utilities.MillimetersToPoints(millimeters);
    }

    public static float PointsToMillimeters(this float points)
    {
        return Utilities.PointsToMillimeters(points);
    }
}