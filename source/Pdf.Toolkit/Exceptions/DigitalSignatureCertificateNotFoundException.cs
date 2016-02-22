using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
