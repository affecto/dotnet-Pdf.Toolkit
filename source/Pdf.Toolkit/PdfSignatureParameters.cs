namespace Affecto.Pdf.Toolkit
{
    public class PdfSignatureParameters
    {
        public const int DefaultHeight = 60;
        public const int DefaultLeftMargin = 50;
        public const int DefaultRightMargin = 50;
        public const int DefaultPageNumber = 1;
        public const int DefaultYLocation = 100;

        /// <summary>
        /// Name of the signature field. Must be unique within the document!
        /// </summary>
        public string SignatureName { get; private set; }

        public int SignatureYLocation { get; set; }
        public int SignatureLeftMargin { get; set; }
        public int SignatureRightMargin { get; set; }
        public int SignatureHeight { get; set; }

        /// <summary>
        /// Number of the page to add the signature to
        /// </summary>
        public int SignaturePageNumber { get; set; }

        /// <summary>
        /// Signed document will be written with this name/path. If null, filename will be generated.
        /// </summary>
        public string TargetFilePath { get; set; }

        public string AdditionalSignatureText { get; set; }

        /// <param name="signatureName">Name of the signature field. Must be unique within the document!</param>
        public PdfSignatureParameters(string signatureName)
        {
            SignatureYLocation = DefaultYLocation;
            SignatureHeight = DefaultHeight;
            SignatureLeftMargin = DefaultLeftMargin;
            SignatureRightMargin = DefaultRightMargin;
            SignaturePageNumber = DefaultPageNumber;

            AdditionalSignatureText = string.Empty;
            SignatureName = signatureName;
        }
        
    }
}
