using System;

namespace Limbo.Integrations.BorgerDk.Exceptions {

    public class BorgerDkNotExportableException : BorgerDkException {

        public string Url { get; }

        public BorgerDkNotExportableException(string url, string message, Exception innerException) : base(message, innerException) {
            Url = url;
        }

        public BorgerDkNotExportableException(string url, Exception innerException) : base(innerException?.Message, innerException) {
            Url = url;
        }

    }

}