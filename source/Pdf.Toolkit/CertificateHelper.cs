using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Affecto.Pdf.Toolkit.Exceptions;
using Affecto.Pdf.Toolkit.Interfaces;
using Org.BouncyCastle.X509;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace Affecto.Pdf.Toolkit
{
    internal static class CertificateHelper
    {
        public static SigningCertificates GetSigningCertificates(IDigitalSignatureCertificateSelector certificateSelector)
        {
            SigningCertificates signingCertificates = new SigningCertificates();

            X509CertificateParser parser = new X509CertificateParser();
            X509Store x509Store = new X509Store(StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection validCertificates = FindDigitalSignatureCertificates(x509Store);

            signingCertificates.X509Certificate2 = certificateSelector.SelectCertificate(validCertificates.Cast<X509Certificate2>());
            signingCertificates.X509Certificate = parser.ReadCertificate(signingCertificates.X509Certificate2.Export(X509ContentType.Cert));
            signingCertificates.FinalChain = CreateChain(signingCertificates.X509Certificate, signingCertificates.X509Certificate2, parser);            

            return signingCertificates;
        }

        public static Dictionary<string, string> GetSubjectFields(X509Certificate x509Certificate)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            string issuer = x509Certificate.SubjectDN.ToString();

            string[] issuerFields = issuer.Split(',');
            foreach (string field in issuerFields)
            {
                string[] fieldSplit = field.Split('=');
                string key = fieldSplit[0].Trim();
                string value = fieldSplit[1].Trim();

                if (!fields.Keys.Contains(key))
                {
                    fields.Add(key, value);
                }
                else
                {
                    fields[key] = value;
                }
            }

            return fields;
        }

        private static X509Certificate2Collection FindDigitalSignatureCertificates(X509Store x509Store)
        {
            X509Certificate2Collection certificates = x509Store.Certificates.Find(X509FindType.FindByKeyUsage, "nonRepudiation", true);
            if (certificates.Count == 0)
            {
                throw new DigitalSignatureCertificateNotFoundException("No certificate with key usage 'nonRepudiation' found.");
            }

            return certificates;
        }

        private static List<X509Certificate> CreateChain(X509Certificate x509Certificate, X509Certificate2 x509Certificate2, X509CertificateParser parser)
        {
            X509Chain chain = new X509Chain(false);
            chain.Build(x509Certificate2);

            List<X509Certificate> finalChain = new List<X509Certificate>();
            foreach (var chainElement in chain.ChainElements)
            {
                chainElement.Certificate.Verify();
                finalChain.Add(parser.ReadCertificate(chainElement.Certificate.Export(X509ContentType.Cert)));
            }

            finalChain.Add(x509Certificate);

            return finalChain;
        }
    }
}
