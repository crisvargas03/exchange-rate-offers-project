using Microsoft.Extensions.Logging;
using OrchestratorMicroService.Infrastructure.Interfaces;
using System.Net.Http.Json;

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
                _logger.LogError(ex, "Unexpected error for {Url}", relativeUrl);
                return default;
            }
        }
    }
}
