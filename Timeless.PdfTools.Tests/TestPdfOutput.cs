using System;
using System.IO;
using NUnit.Framework;

namespace Timeless.PdfTools.Tests
{
    public static class TestPdfOutput
    {
        private static string DirectoryPath
        {
            get
            {
                var outputdir = Environment.GetEnvironmentVariable("PDFTOOLS_TEST_OUTPUT_DIR");
                
                if (outputdir != null)
                {
                    return outputdir;
                }

                var directoryPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestOutput", "Timeless.PdfTools.Lib");
                return directoryPath;
            }
        }

        public static void Write(byte[] bytes, string fileNameWithoutExtension)
        {
            Directory.CreateDirectory(DirectoryPath);

            var path = Path.Combine(DirectoryPath, $"{fileNameWithoutExtension}.pdf");
            File.WriteAllBytes(path, bytes);
            TestContext.AddTestAttachment(path);
        }

        public static void Clear()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(DirectoryPath, "*.pdf"))
            {
                File.Delete(file);
            }
        }
    }

    [SetUpFixture]
    public sealed class TestRunSetup
    {
        [OneTimeSetUp]
        public void BeforeAnyTests()
        {
            TestPdfOutput.Clear();
        }
    }
}
