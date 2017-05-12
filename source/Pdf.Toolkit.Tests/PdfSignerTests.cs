using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Affecto.Pdf.Toolkit.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.Pdf.Toolkit.Tests
{
    [TestClass]
    public class PdfSignerTests
    {
        private IDigitalSignatureCertificateSelector certificateSelector;

        [TestInitialize]
        public void Setup()
        {
            certificateSelector = new FirstCertificateSelector();
        }
        
        /// <summary>
        /// This test can only be run locally when there is a valid digital signing certificate present (like identity card + reader)
        /// </summary>
        [Ignore]
        [TestMethod]
        public void SignFile()
        {
            PdfSigner.SignFile(@"Resources\A4 Test Page.pdf", new PdfSignatureParameters("Digital Signature Test", EncryptionType.Sha256)
                {
                    SignaturePageNumber = 1,
                    SignatureYLocation = 600,
                    SignatureHeight = 60,
                    SignatureTemplate = "Signed by {firstname} {lastname}\non {signdate}",
                    TargetFilePath = @"C:\temp\signed.pdf",
                    SignatureLeftMargin = 50,
                    SignatureRightMargin = 50
                },
                certificateSelector);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SignFile_SourceFileNameNotGiven_Throws()
        {
            PdfSigner.SignFile("", new PdfSignatureParameters("Digital Signature Test", EncryptionType.Sha256), certificateSelector);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignFile_ParametersNotGiven_Throws()
        {
            PdfSigner.SignFile(@"Resources\A4 Test Page.pdf", null, certificateSelector);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SignFile_SourceFileDoesNotExist_Throws()
        {
            PdfSigner.SignFile("foo", new PdfSignatureParameters("Digital Signature Test", EncryptionType.Sha256), certificateSelector);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SignFile_CertificateSelectorNotGiven_Throws()
        {
            PdfSigner.SignFile(@"Resources\A4 Test Page.pdf", new PdfSignatureParameters("Digital Signature Test", EncryptionType.Sha256), null);
        }

        private class FirstCertificateSelector : IDigitalSignatureCertificateSelector
        {
            /// <summary>
            /// Selects certificate for signature from certificate collection
            /// </summary>
            /// <param name="certificates">Certificates that can be used for digital signature (key usage = "nonRepudiation")</param>
            /// <returns>Selected digital signature certificate</returns>
            public X509Certificate2 SelectCertificate(IEnumerable<X509Certificate2> certificates)
            {
                return certificates.First();
            }
        }
    }
}
