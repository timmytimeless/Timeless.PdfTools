# Timeless.PdfTools

Timeless.PdfTools is a small C# wrapper around the older free-to-use version of iTextSharp. It exists to make common PDF generation and PDF-template workflows easier for application developers, without having to work directly with iTextSharp's lower-level APIs for every document.

The main entry point is a fluent `PdfDocumentBuilder<TData>` API. It is especially useful when you need to place content at exact positions on a PDF, because layout rectangles can be specified in millimeters with `PdfLayoutRectangleMm`.

## Why this library exists

iTextSharp is powerful, but its API is fairly low-level when the job is simply:

- load an existing PDF template
- fill fields from C# data
- add positioned text or barcodes
- embed fonts
- flatten the result
- protect the output
- combine generated documents

Timeless.PdfTools wraps those operations in a chainable C# API so document creation reads like a set of PDF build instructions.

## Example

```csharp
var bytes = new PdfDocumentBuilder<MyDocumentData>()
    .FromTemplate(templateBytes)
    .WithEmbeddedFont(arialNovaBytes, "ArialNova")
    .WithField(
        new PdfLayoutRectangleMm(topMm: 50.40f, leftMm: 85.5f, widthMm: 54.25f, heightMm: 11.2f),
        "SampleField",
        "ABC123",
        fontSize: 22f)
    .WithBarcode128(new PdfLayoutRectangleMm(50, 160, 50, 25), "ABC123")
    .Flattened()
    .WithPasswordProtection(ownerPassword)
    .Build(data);
```

## Fluent API overview

The API is intentionally discoverable from the method names. Current builder operations include:

- `FromTemplate(...)` to start from an existing PDF template.
- `WithFilledField(...)` and `WithConditionallyFilledField(...)` for explicit form-field values.
- `WithConventionallyFilledFields()` to fill fields by matching field names to data properties.
- `WithDynamicallyFilledFields()` for dynamic data objects.
- `WithCheckedState(...)` for checkbox fields.
- `WithField(...)` to add a text field at an absolute millimeter-based position.
- `WithBarcode128(...)` and `WithBarcode39(...)` to place barcodes at absolute millimeter-based positions.
- `WithEmbeddedFont(...)` and `WithEmbeddedFontForField(...)` for document-wide or field-specific font embedding.
- `Flattened()` to flatten form fields into the final document.
- `Rotated()` to rotate pages.
- `WithPasswordProtection(...)` and `WithPasswordProtectionAndContentCopyingEnabled(...)` for protected output.

`PdfDocument` also supports appending documents, which is useful when multiple generated PDFs need to be combined into one output file.

## Absolute positioning in millimeters

The `PdfLayoutRectangleMm` type represents a rectangle using:

```csharp
new PdfLayoutRectangleMm(topMm, leftMm, widthMm, heightMm)
```

This keeps document layout code close to real-world measurements. That is useful for labels, forms, barcode templates, pre-printed paper layouts, and other PDFs where content has to land in a precise physical location.

## Tests as examples

The test project contains usage examples for the supported workflows, including:

- loading templates
- filling conventional and dynamic fields
- embedding fonts, including field-specific bold fonts
- rendering Code 128 and Code 39 barcodes
- checking and unchecking PDF checkboxes
- generating password-protected documents
- allowing content copying on protected documents
- rotating multi-page documents
- creating positioned labels
- appending documents

See `Timeless.PdfTools.Tests/PdfDocumentBuilderTests.cs` for the most complete set of examples.
