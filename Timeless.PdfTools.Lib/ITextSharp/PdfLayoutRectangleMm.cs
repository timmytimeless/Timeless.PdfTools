namespace Timeless.PdfTools.Lib.ITextSharp;

public class PdfLayoutRectangleMm
{
	public float TopMm { get; }
	public float LeftMm { get; }
	public float WidthMm { get; }
	public float HeightMm { get; }

	public PdfLayoutRectangleMm(float topMm, float leftMm, float widthMm, float heightMm)
	{
		TopMm = topMm;
		LeftMm = leftMm;
		WidthMm = widthMm;
		HeightMm = heightMm;
	}
}