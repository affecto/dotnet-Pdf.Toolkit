using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace Affecto.Pdf.Toolkit
{
    public static class PdfSigner
    {       
        public static string SignFile(string fileName, PdfSignatureParameters parameters)
        {
            string tempPath = string.Empty;
            try
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

                tempPath = GetTempPath(parameters.TempFolderPath);

                string targetFilePath = GetTargetFilePath(parameters.TempFolderPath, parameters.TargetFilePath);

                var signingCertificates = CertificateHelper.GetSigningCertificates();

                OcspClientBouncyCastle oscpClient = new OcspClientBouncyCastle(null);
                List<ICrlClient> clrClients = new List<ICrlClient> { new CrlClientOnline(signingCertificates.FinalChain) };

                using (FileStream targetFileStream = new FileStream(targetFilePath, FileMode.Create))
                using (PdfReader reader = new PdfReader(fileName))
                {
                    PdfStamper stamper = PdfStamper.CreateSignature(reader, targetFileStream, '0', tempPath, true);
                    PdfSignatureAppearance appearance = GetPdfSignatureAppearance(signingCertificates, stamper, reader, parameters);

                    CreateSignature(signingCertificates, appearance, clrClients, oscpClient);
                }
                
                return targetFilePath;
            }
            finally
            {
                if (!string.IsNullOrWhiteSpace(tempPath) && File.Exists(tempPath))
                {
                    File.Delete(tempPath);
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
            IExternalSignature externalSignature = new X509Certificate2Signature(signingCertificates.X509Certificate2, "SHA-1");

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

            Rectangle cropBox = reader.GetCropBox(1);

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
                .Replace("{lastname}", surName)
                .Replace("{firstname}", givenName)
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