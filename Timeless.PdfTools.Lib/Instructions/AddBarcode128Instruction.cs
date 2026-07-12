using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Timeless.PdfTools.Lib.ITextSharp;

namespace Timeless.PdfTools.Lib.Instructions;

public class AddBarcode128Instruction<TData> : PdfBuildInstruction<TData>
{
    private readonly string _barcodeValue;
    private readonly PdfLayoutRectangleMm _layoutRectangle;

    public AddBarcode128Instruction(PdfLayoutRectangleMm layoutRectangle, string barcodeValue)
    {
        _barcodeValue = barcodeValue;
        _layoutRectangle = layoutRectangle;
    }

    public override byte[] Execute(byte[] pdfBytes, TData data)
    {
        if (pdfBytes == null) { throw new ArgumentNullException(nameof(pdfBytes)); }
        if (pdfBytes.Length == 0) { throw new ArgumentException("Cannot add barcode image to pdf, because the supplied pdf is empty."); }
        if (string.IsNullOrWhiteSpace(_barcodeValue)) return pdfBytes;

        var barcode128 = new Barcode128
        {
            Code = _barcodeValue,
            StartStopText = true,
            ChecksumText = true,
            Font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false),
            BarHeight = 38,
            CodeType = Barcode.CODE128,
            Size = 9f,
        };

        var layoutRectangleTop = _layoutRectangle.TopMm.MillimetersToPoints();
        var layoutRectangleHeight = _layoutRectangle.HeightMm.MillimetersToPoints();
        var layoutRectangleWidth = _layoutRectangle.WidthMm.MillimetersToPoints();
        var layoutRectangleLeft = _layoutRectangle.LeftMm.MillimetersToPoints();

        using (var ms = new MemoryStream())
        {
            var pdfReader = new NormalizedPdfReader(pdfBytes);
            var pdfStamper = new PdfStamper(pdfReader, ms);
            var pageSize = pdfReader.GetNormalizedPageSize(1);

            var image = barcode128
                .CreateImageWithBarcode(pdfStamper.GetOverContent(1), BaseColor.Black, BaseColor.Black);
                
            image.ScaleToFit(layoutRectangleWidth, layoutRectangleHeight);
            image.SetAbsolutePosition(layoutRectangleLeft, pageSize.Height - (layoutRectangleTop + image.ScaledHeight));

            //TestBorderMaker.EnableTestBorder(image);

            pdfStamper.GetOverContent(1).AddImage(image);
            pdfStamper.FormFlattening = true;

            pdfReader.Close();
            pdfStamper.Close();

            return ms.ToArray();
        }
    }
}