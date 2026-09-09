using FluentValidation;

namespace SupportCenter.Application.Features.Sla.CreateSlaPolicy;

public sealed class CreateSlaPolicyCommandValidator
    : AbstractValidator<CreateSlaPolicyCommand>
{
    public CreateSlaPolicyCommandValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty();

        RuleFor(x => x.ResponseTimeMinutes)
            .GreaterThan(0);

        RuleFor(x => x.ResolutionTimeMinutes)
            .GreaterThan(0);
    }
}