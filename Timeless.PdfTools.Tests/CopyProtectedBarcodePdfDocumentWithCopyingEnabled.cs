using Timeless.PdfTools.Lib;
using Timeless.PdfTools.Lib.ITextSharp;
using Timeless.PdfTools.Tests.Properties;
using Timeless.PdfTools.Tests.TestData;


namespace Timeless.PdfTools.Tests
{
    public class CopyProtectedBarcodePdfDocumentWithCopyingEnabled : PdfDocument<BarcodePdfDocumentData>
    {
        public CopyProtectedBarcodePdfDocumentWithCopyingEnabled(BarcodePdfDocumentData data) : base(data) { }

        protected override byte[] OnCreate(BarcodePdfDocumentData data, PdfDocumentBuilder<BarcodePdfDocumentData> builder)
        {
            return builder
                .FromTemplate(Resources.BarcodeTemplate)
                .WithBarcode128(new PdfLayoutRectangleMm(50, 140, 60, 40), data.BarcodeValue)
                .Flattened()
                .WithPasswordProtectionAndContentCopyingEnabled(TestPasswords.OwnerPassword)
                .Build(data);
        }
    }
}
