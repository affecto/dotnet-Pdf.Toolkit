using System;
using System.IO;
using Affecto.Pdf.Toolkit.Exceptions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Affecto.Pdf.Toolkit
{
    public static class PdfMerger
    {
        public static void Merge(string targetFile, params string[] sourceFiles)
        {
            using (FileStream target = new FileStream(targetFile, FileMode.Create))
            {
                Merge(target, source => new PdfReader(source), sourceFiles);
            }
        }

        public static void Merge(Stream target, params Stream[] sources)
        {
            Merge(target, source => new PdfReader(source), sources);
        }

        private static void Merge<TSource>(Stream target, Func<TSource, PdfReader> createPdfReader, params TSource[] sources)
        {
            if (sources.Length == 0)
            {
                throw new ArgumentException("At least one source must be given.");
            }

            Document document = null;
            PdfReader reader = null;
            try
            {
                document = new Document();
                PdfCopy pdf = new PdfCopy(document, target);
                document.Open();

                foreach (TSource file in sources)
                {
                    reader = createPdfReader(file);
                    pdf.AddDocument(reader);
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                reader?.Close();
                throw new PdfMergeFailedException("Failed to merge pdfs", e);
            }
            finally
            {
                document?.Close();
            }
        }
    }
}
