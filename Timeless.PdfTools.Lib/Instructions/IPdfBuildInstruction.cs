namespace Timeless.PdfTools.Lib.Instructions;

public interface IPdfBuildInstruction<in TData>
{
	byte[] Execute(byte[] pdfBytes, TData data);
}