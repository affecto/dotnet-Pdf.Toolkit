using System;
using Affecto.Pdf.Toolkit.Exceptions;
using iTextSharp.text.pdf;

namespace Affecto.Pdf.Toolkit
{
    public class PdfInfo
    {
        public int NumberOfPages { get; private set; }

        public PdfInfo(string file)
        {
            PdfReader reader = null;

            try
            {
                reader = new PdfReader(file);

                NumberOfPages = reader.NumberOfPages;
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
