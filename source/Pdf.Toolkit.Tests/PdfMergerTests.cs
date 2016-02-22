using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Affecto.Pdf.Toolkit.Tests
{
    [TestClass]
    public class PdfMergerTests
    {
        const string MergedFile = "merged.pdf";

        [TestMethod]
        public void Merge_ThreeDocuments_ResultHasThreePages()
        {           
            try
            {
                PdfMerger.Merge(MergedFile, 
                    @"Resources\A4 Test Page.pdf",
                    @"Resources\A3 Test Page.pdf",
                    @"Resources\B5 Test Page.pdf");

                var info = new PdfInfo(MergedFile);

                Assert.AreEqual(3, info.NumberOfPages);                
            }
            finally 
            {
                File.Delete(MergedFile);
            }          
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Merge_NoSourceDocuments_ThrowsException()
        {
            PdfMerger.Merge(MergedFile);
        }
    }
}
