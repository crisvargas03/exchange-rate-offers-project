using Microsoft.Extensions.Logging;
using OrchestratorMicroService.Infrastructure.Interfaces;
using System.Net.Http.Json;
using System.Text;
using System.Xml.Serialization;

namespace OrchestratorMicroService.Infrastructure.Implementations
{
    public class HttpRequestHandler<TClient> : IHttpRequestHandler<TClient> where TClient : class
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpRequestHandler<TClient>> _logger;

        public HttpRequestHandler(IHttpClientFactory factory, ILogger<HttpRequestHandler<TClient>> logger)
        {
            _httpClient = factory.CreateClient(typeof(TClient).Name);
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(relativeUrl, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Request failed [{StatusCode}] for {Url}", response.StatusCode, relativeUrl);
                    return default;
                }

                var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

                if (result == null)
                {
                    _logger.LogWarning("Null response body for {Url}", relativeUrl);
                }

                return result;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex.Message, "Timeout or cancellation for {Url}", relativeUrl);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Unexpected error for {Url}", relativeUrl);
                return default;
            }
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest data, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(relativeUrl, data, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST failed [{StatusCode}] for {Url}", response.StatusCode, relativeUrl);
                    return default;
                }

                return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "POST error for {Url}", relativeUrl);
                return default;
            }
        }

        public async Task<TResponse?> PostXmlAsync<TRequest, TResponse>(string relativeUrl, TRequest data, CancellationToken cancellationToken = default)
        {
            try
            {
                var xml = SerializeToXml(data);

                using var content = new StringContent(xml, Encoding.UTF8, "application/xml");
                var response = await _httpClient.PostAsync(relativeUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("POST XML failed [{StatusCode}] for {Url}", response.StatusCode, relativeUrl);
                    return default;
                }

                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                return DeserializeFromXmlStream<TResponse>(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "POST XML error for {Url}", relativeUrl);
                return default;
            }
        }

        private static string SerializeToXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        private static T? DeserializeFromXmlStream<T>(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T?)serializer.Deserialize(stream);
        }
    }
}
