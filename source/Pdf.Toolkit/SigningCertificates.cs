using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace Affecto.Pdf.Toolkit
{
    internal class SigningCertificates
    {
        public List<X509Certificate> FinalChain { get; set; }
        public X509Certificate X509Certificate { get; set; }
        public X509Certificate2 X509Certificate2 { get; set; }

    }
}
