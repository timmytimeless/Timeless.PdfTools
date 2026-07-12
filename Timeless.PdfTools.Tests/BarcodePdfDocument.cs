using Timeless.PdfTools.Lib;
using Timeless.PdfTools.Lib.ITextSharp;
using Timeless.PdfTools.Tests.Properties;
using Timeless.PdfTools.Tests.TestData;

namespace Timeless.PdfTools.Tests
{
	public class BarcodePdfDocument : PdfDocument<BarcodePdfDocumentData>
	{
		public BarcodePdfDocument(BarcodePdfDocumentData data) : base(data) { }

		protected override byte[] OnCreate(BarcodePdfDocumentData data, PdfDocumentBuilder<BarcodePdfDocumentData> builder)
		{
			return builder
				.FromTemplate(Resources.BarcodeTemplate)
                .WithBarcode128(new PdfLayoutRectangleMm(100, 10, 100, 50), data.BarcodeValue)
				.Flattened()
				.WithPasswordProtection(TestPasswords.OwnerPassword)
				.Build(data);
		}
    }
}
