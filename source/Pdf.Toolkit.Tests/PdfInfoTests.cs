using System;
using Affecto.Pdf.Toolkit.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.Pdf.Toolkit.Tests
{
    [TestClass]
    public class PdfInfoTests
    {
        private PdfInfo info;

        [TestInitialize]
        public void Setup()
        {
            info = new PdfInfo(@"Resources\A3 Test Page.pdf");
        }

        [TestMethod]
        public void NumberOfPages()
        {
            Assert.AreEqual(1, info.NumberOfPages);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadingPdfInfoFailedException))]
        public void FileDoesNotExist_ThrowsException()
        {
           new PdfInfo("foo.pdf");
        }
    }
}
