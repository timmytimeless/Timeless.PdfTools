using System;
using System.Dynamic;
using System.IO;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib.Instructions;

public class FillFieldsWithDynamicDataInstruction<TData> : PdfBuildInstruction<TData>
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
        var properties = (ExpandoObject)Convert.ChangeType(data, typeof(ExpandoObject));

        foreach (var property in properties)
        {
            var propertyValue = property.Value?.ToString();
            var propertyName = property.Key;

            form.SetField(propertyName, propertyValue);
        }
    }
}