using System.Xml.Serialization;

namespace RateOffersAPI2.Helpers
{
    public static class XmlHelper
    {
        public static async Task<T> DeserializeFromBodyAsync<T>(HttpRequest request, string rootElementName = "ExchangeInput")
        {
            ArgumentNullException.ThrowIfNull(request);

            var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootElementName));

            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            Console.WriteLine($"[XML BODY]:\n{body}");

            using var stringReader = new StringReader(body);
            var result = (T?)serializer.Deserialize(stringReader);

            if (result is null)
                throw new InvalidOperationException("Failed to deserialize XML input.");

            return result;
        }
    }
}
