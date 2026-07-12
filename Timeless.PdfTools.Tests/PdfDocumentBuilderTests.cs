using System;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using iTextSharp.text.pdf;
using NUnit.Framework;
using Timeless.PdfTools.Lib;
using Timeless.PdfTools.Lib.ITextSharp;
using Timeless.PdfTools.Tests.Properties;
using Timeless.PdfTools.Tests.TestData;

namespace Timeless.PdfTools.Tests
{
    //[Ignore("Integration tests")]
    public class PdfDocumentBuilderTests
    {
        [Test]
        public void Can_Load_Pdf_From_Template()
        {
            var textFieldsPdfData = new RussianTextFieldsPdfTestData();
            var textFieldsTemplate = Resources.TextFieldsTemplate;

            var bytes = new PdfDocumentBuilder<RussianTextFieldsPdfTestData>()
                .FromTemplate(textFieldsTemplate)
                .Build(textFieldsPdfData);

            Assert.That(bytes, Is.Not.Null);
            TestPdfOutput.Write(bytes, nameof(Can_Load_Pdf_From_Template));
        }

        [Test]
        public void Can_Load_Two_Page_Document_With_Rotation_From_Template()
        {
            var twoPageTemplateBytes = Resources.TwoPageRotationTemplate;

            var bytes = new PdfDocumentBuilder<object>()
                .FromTemplate(twoPageTemplateBytes)
                .Rotated()
                .Build(null);

            Assert.That(bytes, Is.Not.Null);
            TestPdfOutput.Write(bytes, nameof(Can_Load_Two_Page_Document_With_Rotation_From_Template));
        }

        [Test]
        public void Can_Fill_Pdf_With_Manually_Created_Field()
        {
            var textFieldsPdfData = new RussianTextFieldsPdfTestData();

            var bytes = new PdfDocumentBuilder<RussianTextFieldsPdfTestData>()
                .FromTemplate(Resources.BlankTemplate)
                .WithEmbeddedFont(Resources.ArialNova, nameof(Resources.ArialNova))
                .WithField(new PdfLayoutRectangleMm(10, 10, 100, 20), "SampleField", "Sample text", 22f)
                .Flattened()
                .Build(textFieldsPdfData);

            Assert.That(bytes, Is.Not.Null);

            TestPdfOutput.Write(bytes, nameof(Can_Fill_Pdf_With_Manually_Created_Field));
        }
        
        [Test]
        public void Can_Fill_Pdf_With_DynamicallyFilledFields()
        {
            var expandoObject = JsonSerializer.Deserialize<ExpandoObject>(Resources.DynamicTextFieldsData);

            var bytes = new PdfDocumentBuilder<ExpandoObject>()
                .FromTemplate(Resources.TextFieldsTemplate)
                .WithEmbeddedFont(Resources.ArialNova, nameof(Resources.ArialNova))
                .WithDynamicallyFilledFields()
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(expandoObject);

            Assert.That(bytes, Is.Not.Null);

            TestPdfOutput.Write(bytes, nameof(Can_Fill_Pdf_With_DynamicallyFilledFields));
        }

        [Test]
        public void Can_Fill_Pdf_With_ConventionallyFilledFields()
        {
            var textFieldsPdfData = new EnglishTextFieldsPdfTestData();

            var json = JsonSerializer.Serialize(textFieldsPdfData, new JsonSerializerOptions { WriteIndented = true });

            Console.Write(json);

            var bytes = new PdfDocumentBuilder<EnglishTextFieldsPdfTestData>()
                .FromTemplate(Resources.TextFieldsTemplate)
                .WithEmbeddedFont(Resources.ArialNova, nameof(Resources.ArialNova))
                .WithConventionallyFilledFields()
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(textFieldsPdfData);

            Assert.That(bytes, Is.Not.Null);

            TestPdfOutput.Write(bytes, nameof(Can_Fill_Pdf_With_ConventionallyFilledFields));
        }

        [Test]
        public void Can_Render_Barcode128_On_A4_Pdf()
        {
            var data = new BarcodePdfDocumentData { BarcodeValue = "ABC123" };

            var bytes = new PdfDocumentBuilder<BarcodePdfDocumentData>()
                .FromTemplate(Resources.BarcodeTemplate)
                .WithBarcode128(new PdfLayoutRectangleMm(50, 160, 50, 25), data.BarcodeValue)
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(data);

            Assert.That(bytes, Is.Not.Null);

            TestPdfOutput.Write(bytes, nameof(Can_Render_Barcode128_On_A4_Pdf));
        }

        [Test]
        public void Can_Render_Barcode39_On_A4_Pdf()
        {
            var data = new BarcodePdfDocumentData { BarcodeValue = "ABC123" };

            var bytes = new PdfDocumentBuilder<BarcodePdfDocumentData>()
                .FromTemplate(Resources.BarcodeTemplate)
                .WithBarcode39(new PdfLayoutRectangleMm(75, 160, 50, 25), data.BarcodeValue)
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(data);

            Assert.That(bytes, Is.Not.Null);

            TestPdfOutput.Write(bytes, nameof(Can_Render_Barcode39_On_A4_Pdf));
        }

        [Test]
        public void Can_Produce_Password_Protected_Barcode_Document_While_Allowing_Content_Copying()
        {
            var document = new CopyProtectedBarcodePdfDocumentWithCopyingEnabled(new BarcodePdfDocumentData { BarcodeValue = "ABC123" });
            var bytes = document.GetContents();
            Assert.That(bytes, Is.Not.Null);
            Assert.That(CanOpenWithOwnerPassword(bytes, TestPasswords.OwnerPassword), Is.True);

            TestPdfOutput.Write(bytes, nameof(Can_Produce_Password_Protected_Barcode_Document_While_Allowing_Content_Copying));
        }

        [Test]
        public void Can_Check_And_Uncheck_Checkbox_Pdf()
        {
            var data = new CheckBoxTestPdfDocumentData();

            var bytes = new PdfDocumentBuilder<CheckBoxTestPdfDocumentData>()
                .FromTemplate(Resources.checkboxes_test)
                .WithCheckedState("Yes", data.YesChecked)
                .WithCheckedState("No", data.NoChecked)
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(data);

            Assert.That(bytes, Is.Not.Null);

            TestPdfOutput.Write(bytes, nameof(Can_Check_And_Uncheck_Checkbox_Pdf));
        }

        [Test]
        public void Can_Produce_CombinedDocument()
        {
            var barcodeDocument = new BarcodePdfDocument(new BarcodePdfDocumentData { BarcodeValue = "1234-1234-1234" });
            var formFieldDocument = new ConventionallyFilledFieldsPdfDocument(NewConventionallyFilledFieldsPdfDocumentData());
            var combined = barcodeDocument.Append(formFieldDocument);
            var bytes = combined.GetContents();
            Assert.That(bytes, Is.Not.Null);

            TestPdfOutput.Write(bytes, nameof(Can_Produce_CombinedDocument));
        }

        private static ConventionallyFilledFieldsPdfDocumentData NewConventionallyFilledFieldsPdfDocumentData()
        {
            return new ConventionallyFilledFieldsPdfDocumentData
            {
                SampleText = "Sample text"
            };
        }

        [Test]
        public void Can_Produce_Text_Field_Document_With_Embedded_Font_Resource_In_Russian()
        {
            var TextFieldsPdfData = new RussianTextFieldsPdfTestData();

            var bytes = new PdfDocumentBuilder<RussianTextFieldsPdfTestData>()
                .FromTemplate(Resources.TextFieldsTemplate)
                .WithEmbeddedFont(Resources.ArialNova, nameof(Resources.ArialNova))
                .WithConventionallyFilledFields()
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(TextFieldsPdfData);

            Assert.That(bytes, Is.Not.Null);
            TestPdfOutput.Write(bytes, nameof(Can_Produce_Text_Field_Document_With_Embedded_Font_Resource_In_Russian));

        }

        [Test]
        public void Can_Produce_Text_Field_Document_With_Embedded_Font_Resource_In_Russian_Where_A_Single_Field_Is_In_Bold()
        {
            var TextFieldsPdfData = new RussianTextFieldsPdfTestData();

            var bytes = new PdfDocumentBuilder<RussianTextFieldsPdfTestData>()
                .FromTemplate(Resources.TextFieldsTemplate)
                .WithEmbeddedFont(Resources.ArialNova, nameof(Resources.ArialNova))
                .WithEmbeddedFontForField("DocumentTitle", Resources.ArialNova_BoldItalic, nameof(Resources.ArialNova_BoldItalic))
                .WithConventionallyFilledFields()
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(TextFieldsPdfData);

            Assert.That(bytes, Is.Not.Null);

            TestPdfOutput.Write(bytes, nameof(Can_Produce_Text_Field_Document_With_Embedded_Font_Resource_In_Russian_Where_A_Single_Field_Is_In_Bold));
        }

        [Test]
        public void Can_Produce_PositionedLabel_Document()
        {
            var data = new PositionedLabelDocumentData();

            var bytes = new PdfDocumentBuilder<PositionedLabelDocumentData>()
                .FromTemplate(Resources.PositionedLabelTemplate)
                .WithField(new PdfLayoutRectangleMm(50.40f, 85.5f, 54.25f, 11.2f), "SampleField", "ABC123", 22f)
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(data);

            Assert.That(bytes, Is.Not.Null);
            TestPdfOutput.Write(bytes, nameof(Can_Produce_PositionedLabel_Document));
        }


        [Test]
        public void Can_Produce_PositionedBoldLabel_Document()
        {
            var data = new PositionedLabelDocumentData();

            var bytes = new PdfDocumentBuilder<PositionedLabelDocumentData>()
                .FromTemplate(Resources.PositionedBoldLabelTemplate)
                .WithField(new PdfLayoutRectangleMm(66.5f, 73.3f, 118f, 4.6f), "SampleField", "ABC123", 10f)
                .WithEmbeddedFontForField("SampleField", Resources.ArialNova_Bold, nameof(Resources.ArialNova_Bold))
                .Flattened()
                .WithPasswordProtection(TestPasswords.OwnerPassword)
                .Build(data);

            Assert.That(bytes, Is.Not.Null);
            TestPdfOutput.Write(bytes, nameof(Can_Produce_PositionedBoldLabel_Document));
        }

        [Test]
        public void Can_Produce_Barcode_For_Barcode_Printer()
        {
            var bytes = new BarcodeLabelPrinterPdf("ABC123")
                .Create();

            TestPdfOutput.Write(bytes, nameof(Can_Produce_Barcode_For_Barcode_Printer));
        }

        private static bool CanOpenWithOwnerPassword(byte[] bytes, string ownerPassword)
        {
            using (var reader = new PdfReader(bytes, Encoding.UTF8.GetBytes(ownerPassword)))
            {
                return reader.IsEncrypted();
            }
        }
    }
}
