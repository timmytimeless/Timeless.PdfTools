using Timeless.PdfTools.Lib.ITextSharp;

namespace Timeless.PdfTools.Lib.Instructions;

public abstract class PdfBuildInstruction<TData> : IPdfBuildInstruction<TData>
{
    public abstract byte[] Execute(byte[] pdfBytes, TData data);

    /// <summary>
    /// Creates a metricly positioned rectangle.
    /// </summary>
    /// <param name="pageSize">The page size in mm.</param>
    /// <param name="layoutRectangle">The layout rectangle.</param>
    protected NormalizedPdfRectangle CreateMetricPositionedRectangle(NormalizedPdfRectangle pageSize, PdfLayoutRectangleMm layoutRectangle)
    {
        var rectangle = new RectangleJ(pageSize)
        {
            X = layoutRectangle.LeftMm.MillimetersToPoints(),
            Y = CalculateYFromMm(pageSize.Height.PointsToMillimeters(), layoutRectangle.HeightMm, layoutRectangle.TopMm).MillimetersToPoints(),
            Width = layoutRectangle.WidthMm.MillimetersToPoints(),
            Height = layoutRectangle.HeightMm.MillimetersToPoints(),
        };

        return new NormalizedPdfRectangle(rectangle);
    }

    private static float CalculateYFromMm(float pageSizeHeightInMm, float rectangleHeightInMm, float rectangleTopInMm)
    {
        return pageSizeHeightInMm - (rectangleHeightInMm + rectangleTopInMm);
    }
}