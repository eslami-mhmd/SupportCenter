using FluentValidation;

namespace SupportCenter.Application.Features.Tickets.CreateTicket;

public sealed class CreateTicketValidator 
    : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketValidator()
    {
        RuleFor(x => x.OrganizationId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}