namespace OrchestratorMicroService.Infrastructure.Interfaces
{
    public interface IHttpRequestHandler<TClient>
    {
        Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest data, CancellationToken cancellationToken = default);
        Task<TResponse?> PostXmlAsync<TRequest, TResponse>(string relativeUrl, TRequest data, CancellationToken cancellationToken = default);
    }
}
