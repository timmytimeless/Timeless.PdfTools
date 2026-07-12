using System.IO;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class SetFieldCheckedStateInstruction<TData> : PdfBuildInstruction<TData>
{
    private readonly string _fieldName;
    private readonly bool _fieldValue;

    public SetFieldCheckedStateInstruction(string fieldName, bool fieldValue)
    {
        _fieldName = fieldName;
        _fieldValue = fieldValue;
    }

    public override byte[] Execute(byte[] pdfBytes, TData data)
    {
        using (var ms = new MemoryStream())
        {
            var pdfReader = new PdfReader(pdfBytes);
            var pdfStamper = new PdfStamper(pdfReader, ms);
            var fields = pdfStamper.AcroFields;

            fields.SetField(_fieldName, _fieldValue ? "Yes" : "No");
            pdfReader.Close();
            pdfStamper.Close();

            return ms.ToArray();
        }
    }
}