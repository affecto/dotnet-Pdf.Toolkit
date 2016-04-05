using System;
using System.Runtime.Serialization;

namespace Affecto.Pdf.Toolkit.Exceptions
{
    [Serializable]
    public class DigitalSignatureCertificateNotFoundException : Exception
    {
        public DigitalSignatureCertificateNotFoundException()
        {
        }

        public DigitalSignatureCertificateNotFoundException(string message)
            : base(message)
        {
        }

        public DigitalSignatureCertificateNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected DigitalSignatureCertificateNotFoundException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
