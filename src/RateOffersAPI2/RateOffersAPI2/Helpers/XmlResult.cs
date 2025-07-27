using System.Xml.Serialization;

namespace RateOffersAPI2.Helpers
{
    public sealed class XmlResult<T> : IResult
    {
        private static readonly XmlSerializer _serializer = new(typeof(T));
        private readonly T _result;

        public XmlResult(T result)
        {
            _result = result;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            using var stream = new MemoryStream();
            _serializer.Serialize(stream, _result);
            httpContext.Response.ContentType = "application/xml";
            stream.Position = 0;
            await stream.CopyToAsync(httpContext.Response.Body);
        }

    }
}
