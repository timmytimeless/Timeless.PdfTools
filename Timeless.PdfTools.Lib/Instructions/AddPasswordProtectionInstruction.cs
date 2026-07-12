using System.IO;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class AddPasswordProtectionInstruction<TData> : PdfBuildInstruction<TData>
{
	private const int AllowAssistiveTechnology = 512;
	private readonly string _ownerPassword;

	public AddPasswordProtectionInstruction(string ownerPassword)
	{
		_ownerPassword = ownerPassword;
	}

	public override byte[] Execute(byte[] pdfBytes, TData data)
	{
		using (var input = new MemoryStream(pdfBytes))
		{
			using (var output = new MemoryStream())
			{
				var reader = new PdfReader(input);
				PdfEncryptor.Encrypt(reader, output, true, null, _ownerPassword, PdfWriter.ALLOW_PRINTING | AllowAssistiveTechnology);
				return output.ToArray();
			}
		}
	}
}