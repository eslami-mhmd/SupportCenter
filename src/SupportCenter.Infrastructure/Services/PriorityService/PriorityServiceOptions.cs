namespace SupportCenter.Infrastructure.Services.PriorityService;

public class PriorityServiceOptions
{
    public string Endpoint { get; set; } = "https://api.codiv.ai";
    public int TimeoutSeconds { get; set; } = 60;
    public string Model { get; set; } = "openjev-latest";
    public string BearerToken { get; set; } = Environment.GetEnvironmentVariable("PriorityService__BearerToken") ?? string.Empty;
}
