namespace SupportCenter.Infrastructure.Services.PriorityService;

public class PriorityServiceOptions
{
    public string Endpoint { get; set; } = "http://localhost:11434";
    public int TimeoutSeconds { get; set; } = 60;
    public string Model { get; set; } = "laya";
}
