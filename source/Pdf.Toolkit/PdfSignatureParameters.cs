using System.Collections.Generic;

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

        /// <summary>
        /// Template for the signature that appears on the page. Values in format {id} will be replaced.
        /// Available values:
        ///  {firstname} : Signee's first name
        ///  {lastname} : Signee's last name
        ///  {signdate} : Date of the signature
        /// </summary>
        public string SignatureTemplate { get; set; }

        /// <summary>
        /// Temporary folder to use in signing process.
        /// </summary>
        public string TempFolderPath { get; set; }

        /// <summary>
        /// SHA-1, SHA-256 OR SHA-512
        /// </summary>
        public EncryptionType SelectedEncryptionType { get; set; }

        /// <summary>
        /// Should signers name be formatted (Capitalize first letters)
        /// </summary>
        public bool FormatSignerName { get; set; }

        /// <summary>
        /// Use OCSP validation instead of CRL
        /// </summary>
        public bool UseOcsp { get; set; }

        /// <summary>
        /// OCSP URLs for validation
        /// </summary>
        public List<string> OcspUrls { get; set; }

        /// <param name="signatureName">Name of the signature field. Must be unique within the document!</param>
        /// <param name="encryptionType"></param>
        /// <param name="formatSignerName"></param>
        /// <param name="useOcsp"></param>
        /// <param name="ocspUrls"></param>
        public PdfSignatureParameters(string signatureName, EncryptionType encryptionType, bool formatSignerName = true, bool useOcsp = false, List<string> ocspUrls = null)
        {
            SignatureYLocation = DefaultYLocation;
            SignatureHeight = DefaultHeight;
            SignatureLeftMargin = DefaultLeftMargin;
            SignatureRightMargin = DefaultRightMargin;
            SignaturePageNumber = DefaultPageNumber;

            SignatureTemplate = string.Empty;
            SignatureName = signatureName;
            SelectedEncryptionType = encryptionType;
            FormatSignerName = formatSignerName;

            UseOcsp = useOcsp;
            OcspUrls = ocspUrls;
        }
        
    }
}
