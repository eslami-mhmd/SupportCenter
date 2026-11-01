using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace SupportCenter.Infrastructure.Services.PriorityService;

public class TicketPriorityService : Domain.PriorityService.IPriorityService
{
    private readonly HttpClient _httpClient;
    private readonly PriorityServiceOptions _options;

    public TicketPriorityService(HttpClient httpClient, IOptions<PriorityServiceOptions> options)
    {
        _httpClient = httpClient;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<Domain.Tickets.TicketPriority> DeterminePriorityAsync(
        string title,
        string description,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);

        var request = new
        {
            model = _options.Model,
            state = new
            {
                title,
                description
            },
            questions = new
            {
                priority = new
                {
                    type = "choice",
                    instructions = """
                        Determine the appropriate priority for this
                        customer support ticket based on its title
                        and description.

                        Consider business impact, severity, urgency,
                        number of affected users, and whether a
                        workaround exists.

                        Select exactly one priority level.
                        """,
                    criteria = new Dictionary<string, string>
                    {
                        ["Low"] = """
                            Minor issue, cosmetic defect, general
                            question, or feature request.
                            No significant impact on the customer's
                            ability to use the product.
                            """,
                        ["Medium"] = """
                            Moderate issue affecting a limited
                            functionality or individual users.
                            A workaround exists and business
                            operations can continue.
                            """,
                        ["High"] = """
                            Significant functionality is broken,
                            multiple users are affected, or the
                            customer's business operations are
                            seriously disrupted.
                            No reasonable workaround exists.
                            """,
                        ["Critical"] = """
                            Severe incident requiring immediate
                            attention, such as a complete service
                            outage, critical business operation
                            failure, security incident, data loss,
                            or widespread impact on customers.
                            """
                    }
                }
            }
        };

        using var response = await _httpClient.PostAsJsonAsync(
            "/v1/systemone",
            request,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(
                cancellationToken);

            throw new HttpRequestException(
                $"Laya API returned {(int)response.StatusCode}: {error}");
        }

        using var json = await response.Content.ReadFromJsonAsync<JsonDocument>(
            cancellationToken: cancellationToken);

        if (json is null ||
            !json.RootElement
                .TryGetProperty("answers", out var answers) ||
            !answers.TryGetProperty("priority", out var priorityAnswer) ||
            !priorityAnswer.TryGetProperty("choice", out var choice))
        {
            throw new InvalidOperationException(
                "Laya returned an invalid priority response.");
        }

        var priorityValue = choice.GetString();

        if (!System.Enum.TryParse<Domain.Tickets.TicketPriority>(
                priorityValue,
                ignoreCase: true,
                out var priority) ||
            !System.Enum.IsDefined(priority))
        {
            throw new InvalidOperationException(
                $"Laya returned an unknown priority: {priorityValue}");
        }

        return priority;
    }
}
