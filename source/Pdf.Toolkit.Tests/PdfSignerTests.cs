using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.Pdf.Toolkit.Tests
{
    [TestClass]
    public class PdfSignerTests
    {
        /// <summary>
        /// This test can only be run locally when there is a valid digital signing certificate present (like identity card + reader)
        /// </summary>
        [Ignore]
        [TestMethod]
        public void SignFile()
        {
            PdfSigner.SignFile(@"Resources\A4 Test Page.pdf", new PdfSignatureParameters("Digital Signature Test")
                {
                    SignaturePageNumber = 1,
                    SignatureYLocation = 600,
                    SignatureHeight = 60,
                    SignatureTemplate = "Signed by {firstname} {lastname}\non {signdate}",
                    TargetFilePath = @"C:\temp\signed.pdf",
                    SignatureLeftMargin = 50,
                    SignatureRightMargin = 50
                });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SignFile_SourceFileNameNotGiven_Throws()
        {
            PdfSigner.SignFile("", new PdfSignatureParameters("Digital Signature Test"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignFile_ParametersNotGiven_Throws()
        {
            PdfSigner.SignFile(@"Resources\A4 Test Page.pdf", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SignFile_SourceFileDoesNotExist_Throws()
        {
            PdfSigner.SignFile("foo", new PdfSignatureParameters("Digital Signature Test"));
        }
    }
}
