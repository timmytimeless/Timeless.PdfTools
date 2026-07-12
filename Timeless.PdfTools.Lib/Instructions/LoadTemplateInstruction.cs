using System.IO;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class LoadTemplateInstruction<TData> : PdfBuildInstruction<TData>
{
	private readonly byte[] _templateBytes;


	public LoadTemplateInstruction(byte[] templateBytes)
	{
		_templateBytes = templateBytes;
	}


	public override byte[] Execute(byte[] pdfBytes, TData data)
	{
		using (var ms = new MemoryStream())
		{
			var pdfReader = new PdfReader(_templateBytes);
			var pdfStamper = new PdfStamper(pdfReader, ms);
			pdfStamper.Close();
			return ms.ToArray();
		}
	}
}