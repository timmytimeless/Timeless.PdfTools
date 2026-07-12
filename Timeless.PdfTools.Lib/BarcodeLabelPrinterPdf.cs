using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib;

public class BarcodeLabelPrinterPdf
{
    private readonly string _barcodeText;
    private const float PointsPerInch = 72;
    private const float PageWidthInInches = 2.25f;
    private const float PageHeightInInches = 1f;
    public const float PageHeightInPoints = PageHeightInInches * PointsPerInch;
    public const float PageWidthInPoints = PageWidthInInches * PointsPerInch;

    public BarcodeLabelPrinterPdf(string barcodeText)
    {
        _barcodeText = barcodeText;
    }

    public byte[] Create()
    {
        var pageSize = new Rectangle(PageWidthInPoints, PageHeightInPoints);

        return AddBarcodeImageToPdf(
            CreateBlankPdfDocument(pageSize), _barcodeText, pageSize);
    }

    private static byte[] CreateBlankPdfDocument(Rectangle pageSize)
    {
        var document = new Document(pageSize);

        byte[] blankPdfDocument;

        using (var stream = new MemoryStream())
        {
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();
            if (!document.NewPage()) return null;

            document.Add(new Chunk());
                
            document.Close();
            writer.Close();

            blankPdfDocument = stream.ToArray();
        }

        return blankPdfDocument;
    }

    private static byte[] AddBarcodeImageToPdf(byte[] pdfDocumentBytes, string barcodeText, Rectangle targetRectangle)
    {
        var barcode = new Barcode128
        {
            Code = barcodeText,
            StartStopText = true,
            ChecksumText = true,
            Font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false),
            BarHeight = 38,
            Size = 12f,
            Baseline = 15f
        };

        using (var ms = new MemoryStream())
        {
            var pdfReader = new PdfReader(pdfDocumentBytes);
            var pdfStamper = new PdfStamper(pdfReader, ms);
            var pdfContentByte = pdfStamper.GetOverContent(1);
            var image = barcode.CreateImageWithBarcode(pdfContentByte, BaseColor.Black, BaseColor.Black);

            var fit = new Rectangle(
                targetRectangle.Left + 28f,
                targetRectangle.Bottom + 7.2f,
                targetRectangle.Width - 28f,
                targetRectangle.Height - 7.2f
            );

            image.ScaleToFit(fit.Width, fit.Height);
            image.Alignment = Element.ALIGN_CENTER;
            image.SetAbsolutePosition(
                targetRectangle.Left + (targetRectangle.Width - image.ScaledWidth) / 2,
                targetRectangle.Bottom + (targetRectangle.Height - image.ScaledHeight) / 2);

            pdfContentByte.AddImage(image);
            pdfStamper.FormFlattening = true;
            pdfReader.Close();
            pdfStamper.Close();

            return ms.ToArray();
        }
    }
}