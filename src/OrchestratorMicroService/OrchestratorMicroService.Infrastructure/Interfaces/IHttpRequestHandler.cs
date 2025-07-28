namespace OrchestratorMicroService.Infrastructure.Interfaces
{
    public interface IHttpRequestHandler<TClient>
    {
        Task<T?> GetAsync<T>(string relativeUrl, CancellationToken cancellationToken = default);
    }
}
