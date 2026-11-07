namespace SupportCenter.Application.Features.Auth.Login;

public sealed record LoginResult(Guid UserId, string Token);
