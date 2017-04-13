using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Affecto.Pdf.Toolkit.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace Affecto.Pdf.Toolkit
{
    /*
    * For more information on pdf signatures see "Digital Signatures for PDF documents" in http://developers.itextpdf.com/books
    */

    public static class PdfSigner
    {
        public static string SignFile(string fileName, PdfSignatureParameters parameters, IDigitalSignatureCertificateSelector certificateSelector)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Filename must be given", nameof(fileName));
            }
            if (!File.Exists(fileName))
            {
                throw new ArgumentException($"File {fileName} not found.");
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            if (certificateSelector == null)
            {
                throw new ArgumentNullException(nameof(certificateSelector));
            }

            string tempPath = string.Empty;
            try
            {
                tempPath = GetTempPath(parameters.TempFolderPath);

                string targetFilePath = GetTargetFilePath(parameters.TempFolderPath, parameters.TargetFilePath);

                var signingCertificates = CertificateHelper.GetSigningCertificates(certificateSelector);

                // Two clients for checking certificate revocation
                // * Online Certificate Status Protocol (OCSP) client
                // * Certificate Revocation Lists (CRL) client with online checking
                // Certificate will be checked when the signature is made
                OcspClientBouncyCastle oscpClient = new OcspClientBouncyCastle(null);
                List<ICrlClient> clrClients = new List<ICrlClient> { new CrlClientOnline(signingCertificates.FinalChain) };

                using (FileStream targetFileStream = new FileStream(targetFilePath, FileMode.Create))
                using (PdfReader reader = new PdfReader(fileName))
                using (PdfStamper stamper = PdfStamper.CreateSignature(reader, targetFileStream, '0', tempPath, true))
                {
                    stamper.Writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_7);
                    PdfSignatureAppearance appearance = GetPdfSignatureAppearance(signingCertificates, stamper, reader, parameters);
                    CreateSignature(signingCertificates, appearance, clrClients, oscpClient);
                }

                return targetFilePath;
            }
            finally
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(tempPath) && File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }

        private static string GetTempPath(string tempFolderPath)
        {
            if (!string.IsNullOrWhiteSpace(tempFolderPath) && Directory.Exists(tempFolderPath))
            {
                 return Path.Combine(tempFolderPath, Path.GetRandomFileName());
            }

            return Path.GetTempFileName();
        }

        private static string GetTargetFilePath(string tempFolderPath, string targetFilePath)
        {
            if (!string.IsNullOrWhiteSpace(tempFolderPath) && Directory.Exists(tempFolderPath))
            {
                return targetFilePath ?? Path.Combine(tempFolderPath, Path.GetRandomFileName());
            }

            return targetFilePath ?? Path.GetTempFileName();
        }

        private static void CreateSignature(SigningCertificates signingCertificates, PdfSignatureAppearance signatureAppearance, ICollection<ICrlClient> clrClients, IOcspClient oscpClient)
        {
            IExternalSignature externalSignature = new X509Certificate2Signature(signingCertificates.X509Certificate2, "SHA-512");

            MakeSignature.SignDetached(signatureAppearance, externalSignature, signingCertificates.FinalChain, clrClients, oscpClient, null, 0, CryptoStandard.CMS);
        }

        private static PdfSignatureAppearance GetPdfSignatureAppearance(SigningCertificates signingCertificates, PdfStamper stamper, PdfReader reader, PdfSignatureParameters parameters)
        {
            PdfSignatureAppearance appearance = stamper.SignatureAppearance;

            appearance.Reason = "";
            appearance.LocationCaption = "";
            appearance.Location = "";
            appearance.Layer4Text = "";
            appearance.Layer2Text = GetSignatureText(signingCertificates.X509Certificate, parameters);
            appearance.Acro6Layers = true;

            Rectangle cropBox = reader.GetCropBox(parameters.SignaturePageNumber);

            Rectangle rectangle = GetSignatureLocation(cropBox, parameters);
            appearance.SetVisibleSignature(rectangle, parameters.SignaturePageNumber, parameters.SignatureName);

            return appearance;
        }

        private static Rectangle GetSignatureLocation(Rectangle cropBox, PdfSignatureParameters parameters)
        {
            return new Rectangle(cropBox.GetLeft(parameters.SignatureLeftMargin), 
                cropBox.GetTop(parameters.SignatureYLocation + parameters.SignatureHeight), 
                cropBox.Width - parameters.SignatureRightMargin, 
                cropBox.GetTop(parameters.SignatureYLocation));
        }

        private static string GetSignatureText(X509Certificate x509Certificate, PdfSignatureParameters parameters)
        {
            Dictionary<string, string> subjectFields = CertificateHelper.GetSubjectFields(x509Certificate);
            string surName = subjectFields.Keys.Contains("SURNAME") ? subjectFields["SURNAME"] : string.Empty;
            string givenName = subjectFields.Keys.Contains("GIVENNAME") ? subjectFields["GIVENNAME"] : string.Empty;
            string signDate = DateTime.Now.ToString("d.M.yyyy");

            return parameters.SignatureTemplate
                .Replace("{lastname}", UppercaseFirst(surName))
                .Replace("{firstname}", UppercaseFirst(givenName))
                .Replace("{signdate}", signDate);
        }

        private static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1).ToLower();
        }
    }
}