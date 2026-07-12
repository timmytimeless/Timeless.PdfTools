using System;
using System.Collections.Generic;
using Timeless.PdfTools.Lib.Instructions;
using Timeless.PdfTools.Lib.ITextSharp;

namespace Timeless.PdfTools.Lib;

public class PdfDocumentBuilder<TData>
{
    private readonly List<IPdfBuildInstruction<TData>> _instructions = new List<IPdfBuildInstruction<TData>>();

    public PdfDocumentBuilder<TData> FromTemplate(byte[] templateBytes)
    {
        _instructions.Add(new LoadTemplateInstruction<TData>(templateBytes));
        return this;
    }

    public PdfDocumentBuilder<TData> Flattened()
    {
        return WithInstruction(new FlattenFormInstruction<TData>());
    }

    public PdfDocumentBuilder<TData> Rotated()
    {
        return WithInstruction(new RotatePdfInstruction<TData>());
    }

    public PdfDocumentBuilder<TData> WithFilledField(string fieldName, string text)
    {
        return WithInstruction(new FillFieldInstruction<TData>(x => true, fieldName, text));
    }

    public PdfDocumentBuilder<TData> WithCheckedState(string fieldName, bool isChecked)
    {
        return WithInstruction(new SetFieldCheckedStateInstruction<TData>(fieldName, isChecked));
    }

    public PdfDocumentBuilder<TData> WithField(PdfLayoutRectangleMm layoutRectangle, string fieldName, string text, float fontSize)
    {
        return WithInstruction(new AddFieldInstruction<TData>(layoutRectangle, fieldName, text, fontSize));
    }

    public PdfDocumentBuilder<TData> WithConditionallyFilledField(Func<TData, bool> condition, string fieldName, string text)
    {
        return WithInstruction(new FillFieldInstruction<TData>(condition, fieldName, text));
    }

    public PdfDocumentBuilder<TData> WithConventionallyFilledFields()
    {
        return WithInstruction(new FillFieldsByConventionInstruction<TData>());
    }

    public PdfDocumentBuilder<TData> WithDynamicallyFilledFields()
    {
        var instruction = new FillFieldsWithDynamicDataInstruction<TData>();

        return WithInstruction(instruction);
    }

    public PdfDocumentBuilder<TData> WithBarcode128(PdfLayoutRectangleMm layoutRectangle, string barcodeValue)
    {
        return WithInstruction(new AddBarcode128Instruction<TData>(layoutRectangle, barcodeValue));
    }

    public PdfDocumentBuilder<TData> WithBarcode39(PdfLayoutRectangleMm layoutRectangle, string barcodeValue)
    {
        return WithInstruction(new AddBarcode39Instruction<TData>(layoutRectangle, barcodeValue));
    }

    public PdfDocumentBuilder<TData> WithPasswordProtection(string ownerPassword)
    {
        return WithInstruction(new AddPasswordProtectionInstruction<TData>(ownerPassword));
    }

    public PdfDocumentBuilder<TData> WithPasswordProtectionAndContentCopyingEnabled(string ownerPassword)
    {
        return WithInstruction(new AddPasswordProtectionWithCopyEnabledInstruction<TData>(ownerPassword));
    }

    public PdfDocumentBuilder<TData> WithEmbeddedFont(byte[] fontBytes, string fontName)
    {
        return WithInstruction(new AddEmbeddedFontInstruction<TData>(fontBytes, fontName));
    }

    public PdfDocumentBuilder<TData> WithEmbeddedFontForField(string fieldName, byte[] fontBytes, string fontName)
    {
        return WithInstruction(new AddEmbeddedFontForFieldInstruction<TData>(fieldName, fontBytes, fontName));
    }

    public byte[] Build(TData data)
    {
        byte[] documentBytes = null;

        foreach (var instruction in _instructions)
        {
            documentBytes = instruction.Execute(documentBytes, data);
        }

        return documentBytes;
    }

    private PdfDocumentBuilder<TData> WithInstruction(IPdfBuildInstruction<TData> instruction)
    {
        _instructions.Add(instruction);
        return this;
    }

}