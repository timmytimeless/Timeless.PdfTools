using System;
using System.IO;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class FillFieldInstruction<TData> : PdfBuildInstruction<TData>
{
    private readonly Func<TData, bool> _condition;
    private readonly string _fieldName;
    private readonly string _fieldText;

    public FillFieldInstruction(Func<TData, bool> condition, string fieldName, string fieldText)
    {
        _condition = condition;
        _fieldName = fieldName;
        _fieldText = fieldText;
    }

    public override byte[] Execute(byte[] pdfBytes, TData data)
    {
        if (!_condition(data)) return pdfBytes;

        using (var ms = new MemoryStream())
        {
            var pdfReader = new PdfReader(pdfBytes);
            var pdfStamper = new PdfStamper(pdfReader, ms);

            var fields = pdfStamper.AcroFields;
            fields.SetField(_fieldName, _fieldText);

            pdfReader.Close();
            pdfStamper.Close();
            return ms.ToArray();
        }

    }
}