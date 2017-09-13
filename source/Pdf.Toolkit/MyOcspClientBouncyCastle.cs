using System.Collections.Generic;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.X509;

namespace Affecto.Pdf.Toolkit
{
    public class MyOcspClientBouncyCastle : OcspClientBouncyCastle
    {
        private readonly List<string> ocspUrls;

        public MyOcspClientBouncyCastle(OcspVerifier verifier, List<string> ocspUrls) : base(verifier)
        {
            this.ocspUrls = ocspUrls;
        }

        public override BasicOcspResp GetBasicOCSPResp(X509Certificate checkCert, X509Certificate rootCert, string url)
        {
            foreach (string ocspUrl in ocspUrls)
            {
                BasicOcspResp response = base.GetBasicOCSPResp(checkCert, rootCert, ocspUrl);
                if (response != null)
                {
                    return response;
                }
            }

            return null;
        }
    }
}
