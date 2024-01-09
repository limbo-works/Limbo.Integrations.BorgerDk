using System;

namespace Limbo.Integrations.BorgerDk.Exceptions;

public class BorgerDkNotFoundException : BorgerDkException {

    public string Url { get; }

    public BorgerDkNotFoundException(string url, string message, Exception innerException) : base(message, innerException) {
        Url = url;
    }

    public BorgerDkNotFoundException(string url, Exception innerException) : base(innerException.Message, innerException) {
        Url = url;
    }

}