using System.IO;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class FillFieldsByConventionInstruction<TData> : PdfBuildInstruction<TData>
{
    public override byte[] Execute(byte[] templateBytes, TData data)
    {
        using (var ms = new MemoryStream())
        {
            var pdfReader = new PdfReader(templateBytes);
            var pdfStamper = new PdfStamper(pdfReader, ms);

            FillField(pdfStamper.AcroFields, data);
            pdfReader.Close();
            pdfStamper.Close();

            return ms.ToArray();
        }
    }

    protected void FillField(AcroFields form, TData data)
    {
        foreach (var property in typeof(TData).GetProperties())
        {
            var propertyValue = property.GetValue(data)?.ToString() ?? string.Empty;
            var propertyName = property.Name;

            form.SetField(propertyName, propertyValue);
        }
    }
}