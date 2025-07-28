namespace OrchestratorMicroService.Application.Exceptions
{
    public class ProvidersException : Exception
    {
        public string ProviderName { get; }
        public ProvidersException(string providerName, string? message = null)
            : base(message ?? $"Error al consultat Proveedor: {providerName}")
        {
            ProviderName = providerName;
        }
    }
}
