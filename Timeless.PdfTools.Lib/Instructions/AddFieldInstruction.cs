using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Timeless.PdfTools.Lib.ITextSharp;

namespace Timeless.PdfTools.Lib.Instructions;

public class AddFieldInstruction<TData> : PdfBuildInstruction<TData>
{
    private readonly string _fieldName;
    private readonly string _textValue;
    private readonly float _fontSize;
    private readonly PdfLayoutRectangleMm _layoutRectangle;

    public AddFieldInstruction(PdfLayoutRectangleMm layoutRectangle, string fieldName, string textValue, float fontSize)
    {
        _layoutRectangle = layoutRectangle;
        _fieldName = fieldName;
        _textValue = textValue;
        _fontSize = fontSize;
    }

    public override byte[] Execute(byte[] pdfBytes, TData data)
    {
        using (var ms = new MemoryStream())
        {
            var reader = new PdfReader(pdfBytes);
            var pdfStamper = new PdfStamper(reader, ms);
            var pageSize = new NormalizedPdfRectangle(new RectangleJ(reader.GetPageSize(1)));

            var rectangle = CreateMetricPositionedRectangle(pageSize, _layoutRectangle);

            var textField = new TextField(pdfStamper.Writer, rectangle, _fieldName)
            {
                Visibility = BaseField.VISIBLE,
                Text = _textValue,
                Alignment = Element.ALIGN_TOP,
                FontSize = _fontSize,
                Options = BaseField.MULTILINE
            };

            pdfStamper.FormFlattening = true;
            pdfStamper.AddAnnotation(textField.GetTextField(), 1);

            reader.Close();
            pdfStamper.Close();

            return ms.ToArray();
        }
    }
}