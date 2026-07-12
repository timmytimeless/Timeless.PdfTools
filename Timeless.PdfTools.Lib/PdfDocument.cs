using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Timeless.PdfTools.Lib;

public class PdfDocument
{
    private readonly byte[] _contents;

    protected PdfDocument(byte[] contents)
    {
        _contents = contents;
    }

    public virtual byte[] GetContents()
    {
        return _contents;
    }

    public PdfDocument Append(PdfDocument document)
    {
        return Append(new[] { document });
    }

    public PdfDocument Append(IEnumerable<PdfDocument> appendedDocuments)
    {
        byte[] combinedPdfBytes;

        using (var ms = new MemoryStream())
        {
            var pageSize = PageSize.A4;

            var document = new Document();
            var writer = PdfWriter.GetInstance(document, ms);
            document.SetPageSize(pageSize);
            document.SetMargins(0, 0, 0, 0);
            document.Open();

            var pdfContentByte = writer.DirectContent;
            PdfReader.AllowOpenWithFullPermissions = true; //Open PDF while bypassing password protection

            var documentsToWrite = new[] { this }.Union(appendedDocuments).ToList();

            foreach (var pdfReader in documentsToWrite.Select(appendedDocument => new PdfReader(appendedDocument.GetContents())))
            {
                for (var i = 1; i <= pdfReader.NumberOfPages; i++)
                {
                    document.NewPage();
                    var importedPage = writer.GetImportedPage(pdfReader, i);
                    pdfContentByte.AddTemplate(importedPage, 0, 0);
                }
            }

            PdfReader.AllowOpenWithFullPermissions = false;

            document.Close();
            combinedPdfBytes = ms.GetBuffer();
            ms.Flush();
            ms.Dispose();
        }

        return new PdfDocument(combinedPdfBytes);
    }

}


public abstract class PdfDocument<TData> : PdfDocument
{

    private readonly TData _data;
    private byte[] _contents;

    protected PdfDocument(TData data) : base(null)
    {
        _data = data;
    }

    public override byte[] GetContents() =>
        _contents ?? (_contents = OnCreate(_data, new PdfDocumentBuilder<TData>()));

    protected abstract byte[] OnCreate(TData data, PdfDocumentBuilder<TData> builder);
}