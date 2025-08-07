using System.Net;

namespace Semanix.Common.CustomException;

[Serializable]
public class ApplicationException : Exception
{
    public ApplicationException()
    {
    }

    public ApplicationException(string message)
        : base(message)
    {
    }

    public ApplicationException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public ApplicationException(string message, HttpStatusCode statusCode)
        : this(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}