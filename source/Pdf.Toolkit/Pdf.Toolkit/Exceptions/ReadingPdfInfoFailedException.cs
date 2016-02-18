using System;
using System.Runtime.Serialization;

namespace Affecto.Pdf.Toolkit.Exceptions
{
    [Serializable]
    public class ReadingPdfInfoFailedException : Exception
    {

        public ReadingPdfInfoFailedException()
        {
        }

        public ReadingPdfInfoFailedException(string message)
            : base(message)
        {
        }

        public ReadingPdfInfoFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ReadingPdfInfoFailedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
