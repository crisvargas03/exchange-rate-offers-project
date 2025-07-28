namespace OrchestratorMicroService.Application.Options
{
    public class OrchestratorOptions
    {
        public int ProviderTimeoutMilliseconds { get; set; } = 1000;
        public string ExternalApiKey { get; set; } = string.Empty;
    }
}
