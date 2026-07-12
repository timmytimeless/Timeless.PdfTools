using Timeless.PdfTools.Lib;
using Timeless.PdfTools.Tests.Properties;
using Timeless.PdfTools.Tests.TestData;


namespace Timeless.PdfTools.Tests
{
	public class ConventionallyFilledFieldsPdfDocument : PdfDocument<ConventionallyFilledFieldsPdfDocumentData>
	{
		protected override byte[] OnCreate(ConventionallyFilledFieldsPdfDocumentData data, PdfDocumentBuilder<ConventionallyFilledFieldsPdfDocumentData> builder)
		{
			return builder
				.FromTemplate(Resources.ConventionallyFilledFieldsTemplate)
				.WithConventionallyFilledFields()
				.WithConditionallyFilledField(x => x.ShouldFillConditionalField, "ConditionalText", "Sample conditional text")
				.Flattened()
				.WithPasswordProtection(TestPasswords.OwnerPassword)
				.Build(data);
		}

		public ConventionallyFilledFieldsPdfDocument(ConventionallyFilledFieldsPdfDocumentData data) : base(data) { }
	}
}
