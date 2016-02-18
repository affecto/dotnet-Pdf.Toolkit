using System;
using System.Runtime.Serialization;

namespace Affecto.Pdf.Toolkit.Exceptions
{

    [Serializable]
    public class PdfMergeFailedException : Exception
    {
        public PdfMergeFailedException()
        {
        }

        public PdfMergeFailedException(string message)
            : base(message)
        {
        }

        public PdfMergeFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected PdfMergeFailedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
