using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Affecto.Pdf.Toolkit.Exceptions;
using iTextSharp.text.pdf;

namespace Affecto.Pdf.Toolkit
{
    public class PdfInfo
    {
        public int NumberOfPages { get; private set; }
        public IReadOnlyCollection<string> SignatureNames { get; private set; } 

        private readonly PdfReader reader;

        public PdfInfo(string file)
        {     
            try
            {
                reader = new PdfReader(file);

                NumberOfPages = reader.NumberOfPages;
                SignatureNames = reader.AcroFields.GetSignatureNames().AsReadOnly();
            }
            catch (Exception e)
            {
                throw new ReadingPdfInfoFailedException($"Failed to read information from pdf file {file}", e);
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}
