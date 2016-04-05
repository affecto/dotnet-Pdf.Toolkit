using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Affecto.Pdf.Toolkit.Interfaces
{    
    public interface IDigitalSignatureCertificateSelector
    {
        /// <summary>
        /// Selects signing certificate
        /// </summary>
        /// <param name="certificates">Certificates that can be used for digital signature (key usage = "nonRepudiation")</param>
        /// <returns>Selected digital signature certificate</returns>
        X509Certificate2 SelectCertificate(IEnumerable<X509Certificate2> certificates);
    }
}
