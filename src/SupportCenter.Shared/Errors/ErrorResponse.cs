namespace SupportCenter.Shared.Errors;

public sealed record ErrorResponse(
    string Code,
    string Message);